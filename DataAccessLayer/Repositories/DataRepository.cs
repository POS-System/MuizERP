using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using DataAccessLayer.DataReaders;
using DataAccessLayer.Delegates;
using Entities.Base.Attributes;
using Entities.Exceptions.InnerApplicationExceptions;
using IsolationLevel = System.Transactions.IsolationLevel;
using Entities.Exceptions.LogicExceptions;
using Entities.Base;
using MuizEnums;
using System.Linq;
using Entities.Base.Utils;
using Entities.Base.Utils.Interface;
using System.Reflection;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// Класс репозиторий для работы с БД
    /// </summary>
    internal sealed class DataRepository
    {
        [ThreadStatic]
        private static SqlConnection _connectionInTransaction;

        private readonly string _connectionString;
        private readonly IConverter<SqlException, Exception> _sqlExcepionConverter;
        private readonly IKeyedFactory<IEntityCollection, IEnumerable<DataRow>, DataTable> _dataTableFactory;

        public DataRepository(
            string connectionString,
            IConverter<SqlException, Exception> sqlExcepionConverter,
            IKeyedFactory<IEntityCollection, IEnumerable<DataRow>, DataTable> dataTableFactory)
        {
            _connectionString = connectionString;
            _sqlExcepionConverter = sqlExcepionConverter;
            _dataTableFactory = dataTableFactory;
        }

        #region Wrappers

        /// <summary>
        /// Управляет созданием и завершением транзакции, передаваемой делегату inside
        /// </summary>
        /// <param name="inside"></param>
        public void DoInTransaction(InsideTransaction inside)
        {
            var options = new TransactionOptions();
            options.IsolationLevel = IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.MaximumTimeout;

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                DoInConnectionSessionWithTran(conn => inside(conn));
                transactionScope.Complete();
            }
        }

        /// <summary>
        /// Выполняет дествие в контексте соединения с БД
        /// </summary>
        /// <param name="action">Действие выполняемое в контексте соединения БД</param>
        public void DoInConnectionSession(Action<SqlConnection> action)
        {
            if (Transaction.Current != null && _connectionInTransaction == null)
                throw new InvalidOperationException("Попытка использования SqlConnection вне транзакции.");

            if (_connectionInTransaction != null)
                DoInConnectionSessionInsideTransaction(action);
            else
                DoInConnectionSessionOutsideTransaction(action);
        }

        private void DoInConnectionSessionOutsideTransaction(Action<SqlConnection> action)
        {
            using (var connection = CreateSqlConnection())
            {
                try
                {
                    connection.Open();
                    action(connection);
                }
                catch (SqlException sqlException)
                {
                    throw _sqlExcepionConverter.Convert(sqlException);
                }
            }
        }

        private void DoInConnectionSessionInsideTransaction(Action<SqlConnection> action)
        {
            try
            {
                action(_connectionInTransaction);
            }
            catch (SqlException sqlException)
            {
                throw _sqlExcepionConverter.Convert(sqlException);
            }
        }

        private void DoInConnectionSessionWithTran(Action<SqlConnection> action)
        {
            if (_connectionInTransaction != null)
                throw new InvalidOperationException("Попытка создания второго SqlConnection в одной транзакции.");

            try
            {
                _connectionInTransaction = CreateSqlConnection();
                _connectionInTransaction.Open();
                DoInConnectionSessionInsideTransaction(action);
            }
            finally
            {
                if (_connectionInTransaction != null)
                    _connectionInTransaction.Dispose();
                _connectionInTransaction = null;
            }
        }

        #endregion

        #region Read single / first item

        /// <summary>
        /// Выполнение sql команды и чтение первой записи из датаридера в объект.
        /// Если датаридер не содержит ни одной записи, генерируется ошибка <see cref="SqlDataReaderExpectsRecordException"/>.
        /// Если датаридер содержит более одной записи, генерируется ошибка <see cref="SqlDataReaderExpectsOnlyOneRecordException"/>.
        /// </summary>
        /// <typeparam name="T">Тип объекта, в который заполняются поля из датаридера.</typeparam>
        /// <param name="init">Делегат, инициализирующий sql команду.</param>
        /// <param name="read">Делегат, выполняющий чтение данных из датаридера.</param>
        /// <returns>Заполненный из датаридера объект или NULL.</returns>
        public T GetSingleItem<T>(InitSqlCommand init, Func<SqlDataReaderWithSchema, T> read)
        {
            var result = default(T);
            DoInConnectionSession(
                connection =>
                {
                    var sqlCmd = connection.CreateCommand();
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    var type = typeof(T);
                    var loadCommand = GetLoadCommand(type);

                    sqlCmd.CommandText = loadCommand.Name;
                    if (string.IsNullOrEmpty(sqlCmd.CommandText) && !loadCommand.UseEmptyCommandName)
                        sqlCmd.CommandText = $"xp_Get{type.Name}";

                    init(sqlCmd);

                    using (var drd = sqlCmd.ExecuteReader())
                    {
                        if (drd.Read())
                            result = read(new SqlDataReaderWithSchema(drd));
                        else
                            throw new SqlDataReaderExpectsRecordException();

                        if (drd.Read())
                            throw new SqlDataReaderExpectsOnlyOneRecordException();
                    }
                });
            return result;
        }

        /// <summary>
        /// Выполнение sql команды и чтение первой записи из датаридера в объект.
        /// Если датаридер не содержит ни одной записи, возвращает default.
        /// Если датаридер содержит более одной записи, генерируется ошибка <see cref="SqlDataReaderExpectsOnlyOneRecordException"/>.
        /// </summary>
        /// <typeparam name="T">Тип объекта, в который заполняются поля из датаридера.</typeparam>
        /// <param name="init">Делегат, инициализирующий sql команду.</param>
        /// <param name="read">Делегат, выполняющий чтение данных из датаридера.</param>
        /// <returns>Заполненный из датаридера объект или default.</returns>
        public T GetSingleItemOrDefault<T>(InitSqlCommand init, Func<SqlDataReaderWithSchema, T> read)
        {
            try
            {
                return GetSingleItem(init, read);
            }
            catch (SqlDataReaderExpectsRecordException)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Выполнение sql команды и чтение первой записи из датаридера в объект.
        /// Если датаридер не содержит ни одной записи, генерируется логическая ошибка.
        /// Если датаридер содержит более одной записи, генерируется ошибка <see cref="SqlDataReaderExpectsOnlyOneRecordException"/>.
        /// </summary>
        /// <typeparam name="T">Тип объекта, в который заполняются поля из датаридера.</typeparam>
        /// <param name="init">Делегат, инициализирующий sql команду.</param>
        /// <param name="read">Делегат, выполняющий чтение данных из датаридера.</param>
        /// <returns>Заполненный из датаридера объект.</returns>
        public T GetSingleItemOrThrowLogicException<T>(InitSqlCommand init, Func<SqlDataReaderWithSchema, T> read)
        {
            try
            {
                return GetSingleItem(init, read);
            }
            catch (SqlDataReaderExpectsRecordException)
            {
                throw new LogicException("Запрашиваемый объект не найден.\nВозможно объект был удален другим пользователем.\nПопробуйте обновить данные.");
            }
        }

        /// <summary>
        /// Выполнение sql команды и чтение первой записи из датаридера.
        /// </summary>
        /// <param name="init"></param>
        /// <param name="read"></param>
        /// <param name="source"></param>
        public void ReadFirstItem(InitSqlCommand init, ReadDataReaderWithSchema read)
        {
            DoInConnectionSession(
                connection =>
                {
                    var sqlCmd = connection.CreateCommand();
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    init(sqlCmd);

                    using (var drd = sqlCmd.ExecuteReader())
                    {
                        if (!drd.Read())
                            return;
                        var drdWithSchema = new SqlDataReaderWithSchema(drd);
                        read(drdWithSchema);
                    }
                });
        }

        #endregion

        #region Read collection

        /// <summary>
        /// Выполнение sql команды и чтение записей в коллекцию.
        /// </summary>
        /// <typeparam name="TCollection">Тип коллекции.</typeparam>
        /// <typeparam name="TItem">Тип элемента коллекции.</typeparam>
        /// <param name="createCollection">Делегат создающий экземпляр коллекции.</param>
        /// <param name="init">Делегат, инициализирующий sql команду.</param>
        /// <param name="read">Делегат, выполняющий чтение данных из датаридера в объект. И добавляющий объект в коллекцию.</param>
        /// <param name="source">Источник подключения.</param>
        /// <returns>Результирующая коллекция.</returns>
        public TCollection GetCollectionWithSchema<TCollection, TItem>(
            Func<TCollection> createCollection,
            InitSqlCommand init,
            Action<SqlDataReaderWithSchema, TCollection> read)
            where TCollection : IEnumerable<TItem>
        {
            var result = createCollection();

            DoInConnectionSession(
                conn =>
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    init(cmd);

                    using (var drd = cmd.ExecuteReader())
                    {
                        var drdWrapper = new SqlDataReaderWithSchema(drd);

                        while (drd.Read())
                        {
                            read(drdWrapper, result);
                        }
                    }
                });

            return result;
        }

        /// <summary>
        /// Выполнение sql команды и чтение записей в коллекцию.
        /// </summary>
        /// <typeparam name="TCollection">Тип коллекции.</typeparam>
        /// <typeparam name="TItem">Тип элемента коллекции.</typeparam>
        /// <param name="createCollection">Делегат создающий экземпляр коллекции.</param>
        /// <param name="commandName">Название хранимой процедуры для выолнения.</param>
        /// <param name="read">Делегат, выполняющий чтение данных из датаридера в объект. И добавляющий объект в коллекцию.</param>
        /// <returns>Результирующая коллекция.</returns>
        public TCollection GetCollectionWithSchema<TCollection, TItem>(
            Func<TCollection> createCollection,
            string commandName,
            Action<SqlDataReaderWithSchema, TCollection> read) where TCollection : IEnumerable<TItem>
        {
            return GetCollectionWithSchema<TCollection, TItem>(
                createCollection,
                cmd => cmd.CommandText = commandName,
                read);
        }

        /// <summary>
        /// Выполнение sql команды и чтение записей в коллекцию.
        /// </summary>
        /// <param name="init">Делегат, инициализирующий sql команду.</param>
        /// <param name="read">Делегат, выполняющий чтение данных из датаридера в объект. И добавляющий объект в коллекцию.</param>
        public void ReadCollectionWithSchema<T>(InitSqlCommand init, ReadDataReaderWithSchema read)
        {
            DoInConnectionSession(
                connection =>
                {
                    var sqlCmd = connection.CreateCommand();
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    var type = typeof(T);
                    var loadCommand = GetLoadCommand(type);

                    sqlCmd.CommandText = loadCommand.Name;
                    if (string.IsNullOrEmpty(sqlCmd.CommandText) && !loadCommand.UseEmptyCommandName)
                        sqlCmd.CommandText = $"xp_Get{type.Name}";

                    var hierarchyCommand = type.GetCustomAttribute<HierarhyCommnadAttribute>();
                    if (hierarchyCommand != null)
                    {
                        var direction = hierarchyCommand.Direction;
                        sqlCmd.Parameters.AddWithValue("@p_Direction", direction);
                    }

                    init(sqlCmd);

                    using (var drd = sqlCmd.ExecuteReader())
                    {
                        var drdWrapper = new SqlDataReaderWithSchema(drd);

                        while (drd.Read())
                        {
                            read(drdWrapper);
                        }
                    }
                });
        }

        /// <summary>
        /// Выполнение sql команды и чтение записей в коллекцию.
        /// </summary>
        /// <param name="commandName">Название хранимой процедуры для выолнения.</param>
        /// <param name="read">Делегат, выполняющий чтение данных из датаридера в объект. И добавляющий объект в коллекцию.</param>
        public void ReadCollectionWithSchema<T>(string commandName, ReadDataReaderWithSchema read)
        {
            ReadCollectionWithSchema<T>(sqlCmd => { sqlCmd.CommandText = commandName; }, read);
        }

        #endregion

        #region Save item

        /// <summary>
        /// Сохраняет BaseItem-объект, используя переданный объект хранимой процедуры. По завершении выставляет объекту статус ItemStates.None.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="Cmd"></param>
        public void SaveBaseItem(SqlCommand Cmd)
        {
            Cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Сохраняет BaseItem-объект, используя делегат prepare для описания параметров хранимой процедуры
        /// </summary>
        /// <param name="item"></param>
        /// <param name="connection"></param>
        /// <param name="prepare"></param>
        public void SaveBaseItem(SqlConnection connection, PrepareSqlCommand prepare)
        {
            var sqlCmd = prepare(connection);
            SaveBaseItem(sqlCmd);
        }

        /// <summary>
        /// Сохраняет BaseItem-объект, используя свойства, помеченные атрибутом [SaveCommand], для формирования хранимой процедуры
        /// </summary>
        /// <param name="item"></param>
        /// <param name="connection"></param>
        /// <param name="configureSqlCommand"></param>
        public void SaveBaseItem(BaseEntity item, SqlConnection connection, ConfigureSqlCommand configureSqlCommand = null)
        {
            if (!item.IsModified) return;

            var sqlCmd = CreateSaveStoredProcedure(item, connection);

            if (configureSqlCommand != null)
                configureSqlCommand(sqlCmd);

            SaveBaseItem(sqlCmd);

            var parameterId = sqlCmd.Parameters["@p_ID"];
            if (parameterId.Direction == ParameterDirection.InputOutput)
                item.ID = (int)parameterId.Value;
        }

        #endregion

        #region Save collection

        /// <summary>
        /// Сохраняет коллекцию объектов по одному объекту.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="collection">Коллекция сохраняемых объектов.</param>
        /// <param name="itemSaveAction">Делегат для сохранения каждого объекта коллекции.</param>
        public void SaveCollection<T>(IEntityCollection<T> collection, Action<T> itemSaveAction)
            where T : BaseEntity
        {
            if (!collection.IsModified) return;

            foreach (var item in collection)
                itemSaveAction(item);
        }

        /// <summary>
        /// Сохраняет коллекцию объектов с помощью DataTable.
        /// </summary>
        /// <param name="collection">Коллекция объектов</param>
        /// <param name="configureSqlCommand">Делегат для конфигурирования sql-команды.</param>
        public void SaveCollectionWithDataTable(IEntityCollection collection, ConfigureSqlCommand configureSqlCommand = null)
        {
            if (!collection.IsModified) return;

            DoInConnectionSession(
                conn =>
                {
                    var cmd = CreateSaveStoredProcedure(collection, conn);

                    if (configureSqlCommand != null)
                        configureSqlCommand(cmd);

                    cmd.ExecuteNonQuery();
                    
                });
        }

        #endregion

        private SqlCommand CreateSaveStoredProcedure(IEntityCollection collection, SqlConnection connection)
        {
            var type = collection.GetType().GetGenericArguments()[0];

            var saveCommand = GetSaveCommand(type);

            var sqlCmd = connection.CreateCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = $"xp_SaveMerge_{type.Name}";
            sqlCmd.CommandTimeout = saveCommand.Timeout;

            var tableTypeName = type.Name + "Type";

            var schema = GetTableTypeSchema(connection, tableTypeName);

            var table = _dataTableFactory.Create(collection, schema);

            sqlCmd.Parameters.Add(
                new SqlParameter()
                {
                    ParameterName = $"@p_{tableTypeName}",
                    SqlDbType = SqlDbType.Structured,
                    TypeName = $"dbo.{tableTypeName}",
                    Value = table
                });

            return sqlCmd;
        }

        /// <summary>
        /// Метод создания SqlCommand с заполнением параметров для сохранения объекта через рефлексию
        /// </summary>
        /// <param name="connection">Покдлючение к БД</param>
        /// <param name="item">Сохраняемый объект</param>
        /// <returns></returns>
        private SqlCommand CreateSaveStoredProcedure(BaseEntity item, SqlConnection connection)
        {
            var sqlCmd = connection.CreateCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;

            var type = item.GetType();

            var saveCommand = GetSaveCommand(type);

            sqlCmd.CommandText = saveCommand.Name;

            if (string.IsNullOrEmpty(sqlCmd.CommandText) && !saveCommand.UseEmptyCommandName)
                sqlCmd.CommandText = $"xp_Save{type.Name}";

            sqlCmd.CommandTimeout = saveCommand.Timeout;

            var props = type.GetProperties();
            var ignoreProps = saveCommand.IgnoreProperties;

            foreach (var property in props)
            {
                var parameter = property.GetCustomAttribute<SaveParameterAttribute>();
                // если свойство не нуждается в сохранении, пропускаем его
                if (parameter == null) continue;

                // Если свойство присутствует в списке игнорируемых -
                // не добавляем его к параметрам хранимой процедуры
                if (ignoreProps.Contains(property.Name))
                    continue;

                // Получаем название параметра
                var parameterName = parameter.Name;

                // Если не задано, используем название свойства с префиксом @p_
                if (string.IsNullOrEmpty(parameterName))
                    parameterName = $"@p_{property.Name}";

                // настраиваем параметр
                var value = property.GetValue(item, null);

                // Если объект удаляется - делаем его идентификатор отрицательным
                if (CheckValueIsIdentifier(property, item) && item.State == EState.Delete)
                    value = Math.Abs((int)value) * (-1);

                if (CheckValueIsNull(value, parameter))
                {
                    if (parameter.Nullable)
                        value = DBNull.Value;
                    else
                        throw new NoNullAllowedException(
                            string.Format("Объект '{0}' не поддерживает NULL значение для параметра '{1}'.", type, property.Name));
                }

                var sqlParameter = new SqlParameter(parameterName, value);

                if (property.PropertyType == typeof(DateTime))
                    sqlParameter.SqlDbType = SqlDbType.DateTime;

                if (property.PropertyType == typeof(byte[]))
                    sqlParameter.SqlDbType = SqlDbType.Image;

                if (parameter.Size > 0)
                    sqlParameter.Size = parameter.Size;

                sqlParameter.Direction = parameter.Direction;

                // добавляем параметр
                sqlCmd.Parameters.Add(sqlParameter);
            }

            return sqlCmd;
        }

        private static bool CheckValueIsNull(object value, SaveParameterAttribute parameter)
        {
            return (value == null && parameter.NullValue == null) ||
                   (value != null && value.Equals(parameter.NullValue)) ||
                   (value is DateTime && DateTime.MinValue == (DateTime)value) ||
                   (value is decimal && parameter.NullValue != null && value.Equals(Convert.ToDecimal(parameter.NullValue)));
        }

        private static bool CheckValueIsIdentifier(PropertyInfo property, BaseEntity item)
        {
            return property.Name == nameof(item.ID) &&
                property.PropertyType == typeof(int);
        }

        private LoadCommandAttribute GetLoadCommand(Type type)
        {
            var attribute = type.GetCustomAttribute<LoadCommandAttribute>();
            if (attribute == null)
                throw new GeneratingStoredProcedureNotSupportedException(
                    string.Format("Объект '{0}' не поддерживает генерацию команды загрузка." +
                        "Отсутствует [LoadCommandAttribute] аттрибут.", type));

            return attribute;
        }

        private SaveCommandAttribute GetSaveCommand(Type type)
        {
            var attribute = type.GetCustomAttribute<SaveCommandAttribute>();

            if (attribute == null)
                throw new GeneratingStoredProcedureNotSupportedException(
                    string.Format("Объект '{0}' не поддерживает генерацию команды сохранения. Отсутствует [SaveCommandAttribute] аттрибут.", type));

            return attribute;
        }

        private IEnumerable<DataRow> GetTableTypeSchema(SqlConnection connection, string tableTypeName)
        {
            var schema = connection.GetSchema("StructuredTypeMembers")
                .AsEnumerable()
                .Where(r => r.ItemArray[2].Equals(tableTypeName))
                .OrderBy(r => r.ItemArray[4]);

            if (schema == null || schema.Count() == 0)
                throw new SqlServerTypeSchemaNotFoundException(
                    $"Схема данных для типа {tableTypeName} не найдена.");

            return schema;
        }

        private SqlConnection CreateSqlConnection()
        {
            //return new SqlConnection(@"packet size=4096; data source=SERV72\TAMUZ; persist security info=True; initial catalog=JDB_ERP_1; Integrated Security = SSPI;");
            return new SqlConnection(_connectionString);
        }
    }
}