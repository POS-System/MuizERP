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
    /// ����� ����������� ��� ������ � ��
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
        /// ��������� ��������� � ����������� ����������, ������������ �������� inside
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
        /// ��������� ������� � ��������� ���������� � ��
        /// </summary>
        /// <param name="action">�������� ����������� � ��������� ���������� ��</param>
        public void DoInConnectionSession(Action<SqlConnection> action)
        {
            if (Transaction.Current != null && _connectionInTransaction == null)
                throw new InvalidOperationException("������� ������������� SqlConnection ��� ����������.");

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
                throw new InvalidOperationException("������� �������� ������� SqlConnection � ����� ����������.");

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
        /// ���������� sql ������� � ������ ������ ������ �� ���������� � ������.
        /// ���� ��������� �� �������� �� ����� ������, ������������ ������ <see cref="SqlDataReaderExpectsRecordException"/>.
        /// ���� ��������� �������� ����� ����� ������, ������������ ������ <see cref="SqlDataReaderExpectsOnlyOneRecordException"/>.
        /// </summary>
        /// <typeparam name="T">��� �������, � ������� ����������� ���� �� ����������.</typeparam>
        /// <param name="init">�������, ���������������� sql �������.</param>
        /// <param name="read">�������, ����������� ������ ������ �� ����������.</param>
        /// <returns>����������� �� ���������� ������ ��� NULL.</returns>
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
        /// ���������� sql ������� � ������ ������ ������ �� ���������� � ������.
        /// ���� ��������� �� �������� �� ����� ������, ���������� default.
        /// ���� ��������� �������� ����� ����� ������, ������������ ������ <see cref="SqlDataReaderExpectsOnlyOneRecordException"/>.
        /// </summary>
        /// <typeparam name="T">��� �������, � ������� ����������� ���� �� ����������.</typeparam>
        /// <param name="init">�������, ���������������� sql �������.</param>
        /// <param name="read">�������, ����������� ������ ������ �� ����������.</param>
        /// <returns>����������� �� ���������� ������ ��� default.</returns>
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
        /// ���������� sql ������� � ������ ������ ������ �� ���������� � ������.
        /// ���� ��������� �� �������� �� ����� ������, ������������ ���������� ������.
        /// ���� ��������� �������� ����� ����� ������, ������������ ������ <see cref="SqlDataReaderExpectsOnlyOneRecordException"/>.
        /// </summary>
        /// <typeparam name="T">��� �������, � ������� ����������� ���� �� ����������.</typeparam>
        /// <param name="init">�������, ���������������� sql �������.</param>
        /// <param name="read">�������, ����������� ������ ������ �� ����������.</param>
        /// <returns>����������� �� ���������� ������.</returns>
        public T GetSingleItemOrThrowLogicException<T>(InitSqlCommand init, Func<SqlDataReaderWithSchema, T> read)
        {
            try
            {
                return GetSingleItem(init, read);
            }
            catch (SqlDataReaderExpectsRecordException)
            {
                throw new LogicException("������������� ������ �� ������.\n�������� ������ ��� ������ ������ �������������.\n���������� �������� ������.");
            }
        }

        /// <summary>
        /// ���������� sql ������� � ������ ������ ������ �� ����������.
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
        /// ���������� sql ������� � ������ ������� � ���������.
        /// </summary>
        /// <typeparam name="TCollection">��� ���������.</typeparam>
        /// <typeparam name="TItem">��� �������� ���������.</typeparam>
        /// <param name="createCollection">������� ��������� ��������� ���������.</param>
        /// <param name="init">�������, ���������������� sql �������.</param>
        /// <param name="read">�������, ����������� ������ ������ �� ���������� � ������. � ����������� ������ � ���������.</param>
        /// <param name="source">�������� �����������.</param>
        /// <returns>�������������� ���������.</returns>
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
        /// ���������� sql ������� � ������ ������� � ���������.
        /// </summary>
        /// <typeparam name="TCollection">��� ���������.</typeparam>
        /// <typeparam name="TItem">��� �������� ���������.</typeparam>
        /// <param name="createCollection">������� ��������� ��������� ���������.</param>
        /// <param name="commandName">�������� �������� ��������� ��� ���������.</param>
        /// <param name="read">�������, ����������� ������ ������ �� ���������� � ������. � ����������� ������ � ���������.</param>
        /// <returns>�������������� ���������.</returns>
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
        /// ���������� sql ������� � ������ ������� � ���������.
        /// </summary>
        /// <param name="init">�������, ���������������� sql �������.</param>
        /// <param name="read">�������, ����������� ������ ������ �� ���������� � ������. � ����������� ������ � ���������.</param>
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
        /// ���������� sql ������� � ������ ������� � ���������.
        /// </summary>
        /// <param name="commandName">�������� �������� ��������� ��� ���������.</param>
        /// <param name="read">�������, ����������� ������ ������ �� ���������� � ������. � ����������� ������ � ���������.</param>
        public void ReadCollectionWithSchema<T>(string commandName, ReadDataReaderWithSchema read)
        {
            ReadCollectionWithSchema<T>(sqlCmd => { sqlCmd.CommandText = commandName; }, read);
        }

        #endregion

        #region Save item

        /// <summary>
        /// ��������� BaseItem-������, ��������� ���������� ������ �������� ���������. �� ���������� ���������� ������� ������ ItemStates.None.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="Cmd"></param>
        public void SaveBaseItem(SqlCommand Cmd)
        {
            Cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// ��������� BaseItem-������, ��������� ������� prepare ��� �������� ���������� �������� ���������
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
        /// ��������� BaseItem-������, ��������� ��������, ���������� ��������� [SaveCommand], ��� ������������ �������� ���������
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
        /// ��������� ��������� �������� �� ������ �������.
        /// </summary>
        /// <typeparam name="T">��� �������.</typeparam>
        /// <param name="collection">��������� ����������� ��������.</param>
        /// <param name="itemSaveAction">������� ��� ���������� ������� ������� ���������.</param>
        public void SaveCollection<T>(IEntityCollection<T> collection, Action<T> itemSaveAction)
            where T : BaseEntity
        {
            if (!collection.IsModified) return;

            foreach (var item in collection)
                itemSaveAction(item);
        }

        /// <summary>
        /// ��������� ��������� �������� � ������� DataTable.
        /// </summary>
        /// <param name="collection">��������� ��������</param>
        /// <param name="configureSqlCommand">������� ��� ���������������� sql-�������.</param>
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
        /// ����� �������� SqlCommand � ����������� ���������� ��� ���������� ������� ����� ���������
        /// </summary>
        /// <param name="connection">����������� � ��</param>
        /// <param name="item">����������� ������</param>
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
                // ���� �������� �� ��������� � ����������, ���������� ���
                if (parameter == null) continue;

                // ���� �������� ������������ � ������ ������������ -
                // �� ��������� ��� � ���������� �������� ���������
                if (ignoreProps.Contains(property.Name))
                    continue;

                // �������� �������� ���������
                var parameterName = parameter.Name;

                // ���� �� ������, ���������� �������� �������� � ��������� @p_
                if (string.IsNullOrEmpty(parameterName))
                    parameterName = $"@p_{property.Name}";

                // ����������� ��������
                var value = property.GetValue(item, null);

                // ���� ������ ��������� - ������ ��� ������������� �������������
                if (CheckValueIsIdentifier(property, item) && item.State == EState.Delete)
                    value = Math.Abs((int)value) * (-1);

                if (CheckValueIsNull(value, parameter))
                {
                    if (parameter.Nullable)
                        value = DBNull.Value;
                    else
                        throw new NoNullAllowedException(
                            string.Format("������ '{0}' �� ������������ NULL �������� ��� ��������� '{1}'.", type, property.Name));
                }

                var sqlParameter = new SqlParameter(parameterName, value);

                if (property.PropertyType == typeof(DateTime))
                    sqlParameter.SqlDbType = SqlDbType.DateTime;

                if (property.PropertyType == typeof(byte[]))
                    sqlParameter.SqlDbType = SqlDbType.Image;

                if (parameter.Size > 0)
                    sqlParameter.Size = parameter.Size;

                sqlParameter.Direction = parameter.Direction;

                // ��������� ��������
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
                    string.Format("������ '{0}' �� ������������ ��������� ������� ��������." +
                        "����������� [LoadCommandAttribute] ��������.", type));

            return attribute;
        }

        private SaveCommandAttribute GetSaveCommand(Type type)
        {
            var attribute = type.GetCustomAttribute<SaveCommandAttribute>();

            if (attribute == null)
                throw new GeneratingStoredProcedureNotSupportedException(
                    string.Format("������ '{0}' �� ������������ ��������� ������� ����������. ����������� [SaveCommandAttribute] ��������.", type));

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
                    $"����� ������ ��� ���� {tableTypeName} �� �������.");

            return schema;
        }

        private SqlConnection CreateSqlConnection()
        {
            //return new SqlConnection(@"packet size=4096; data source=SERV72\TAMUZ; persist security info=True; initial catalog=JDB_ERP_1; Integrated Security = SSPI;");
            return new SqlConnection(_connectionString);
        }
    }
}