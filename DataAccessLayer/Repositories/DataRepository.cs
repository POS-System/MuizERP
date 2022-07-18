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

        private readonly IConverter<SqlException, Exception> _sqlExcepionConverter;

        private readonly string _connectionString;

        public DataRepository(
            string connectionString,
            IConverter<SqlException, Exception> sqlExcepionConverter)
        {
            _connectionString = connectionString;
            _sqlExcepionConverter = sqlExcepionConverter;
        }

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
            Action<SqlDataReaderWithSchema, TCollection> read) where TCollection : IEnumerable<TItem>
        {
            var result = createCollection();

            DoInConnectionSession(sqlConn =>
            {
                var sqlCmd = sqlConn.CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                init(sqlCmd);

                using (var drd = sqlCmd.ExecuteReader())
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
            return GetCollectionWithSchema<TCollection, TItem>(createCollection,
                sqlCmd => { sqlCmd.CommandText = commandName; }, read);
        }

        /// <summary>
        /// ���������� sql ������� � ������ ������� � ���������.
        /// </summary>
        /// <param name="init">�������, ���������������� sql �������.</param>
        /// <param name="read">�������, ����������� ������ ������ �� ���������� � ������. � ����������� ������ � ���������.</param>
        public void ReadCollectionWithSchema<T>(InitSqlCommand init, ReadDataReaderWithSchema read)
        {
            DoInConnectionSession(sqlConn =>
            {
                var sqlCmd = sqlConn.CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;

                var type = typeof(T);
                var loadCommand = type.GetCustomAttribute<LoadCommandAttribute>();
                if (loadCommand == null)
                    throw new GeneratingStoredProcedureNotSupportedException(
                        string.Format("������ '{0}' �� ������������ ��������� ������� ��������." +
                            "����������� [LoadCommandAttribute] ��������.", type));
          
                sqlCmd.CommandText = loadCommand.Name;
                if (string.IsNullOrEmpty(sqlCmd.CommandText) && !loadCommand.UseEmptyCommandName)
                    sqlCmd.CommandText = $"xp_Get{type.Name}";

                var hierarchyCommand = type.GetCustomAttribute<HierarhyCommnadAttribute>();
                if (hierarchyCommand != null)
                {
                    var directionUp = hierarchyCommand.Direction == EHierarchyDirection.Up;
                    sqlCmd.Parameters.AddWithValue("@p_directionUp", directionUp);
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
            DoInConnectionSession(sqlConn =>
            {
                var sqlCmd = sqlConn.CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;

                var type = typeof(T);
                var loadCommand = type.GetCustomAttribute<LoadCommandAttribute>();
                if (loadCommand == null)
                    throw new GeneratingStoredProcedureNotSupportedException(
                        string.Format("������ '{0}' �� ������������ ��������� ������� ��������." +
                            "����������� [LoadCommandAttribute] ��������.", type));

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
            DoInConnectionSession(sqlConn =>
            {
                var sqlCmd = sqlConn.CreateCommand();
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

        /// <summary>
        /// ��������� BaseItem-������, ��������� ���������� ������ �������� ���������. �� ���������� ���������� ������� ������ ItemStates.None.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="sqlSetCmd"></param>
        public void SaveBaseItem(SqlCommand sqlSetCmd)
        {
            sqlSetCmd.ExecuteNonQuery();
        }

        /// <summary>
        /// ��������� BaseItem-������, ��������� ������� prepare ��� �������� ���������� �������� ���������
        /// </summary>
        /// <param name="item"></param>
        /// <param name="connection"></param>
        /// <param name="prepare"></param>
        public void SaveBaseItem(SqlConnection connection, PrepareSetCommand prepare)
        {
            var sqlSetCmd = prepare(connection);
            SaveBaseItem(sqlSetCmd);
        }

        /// <summary>
        /// ��������� BaseItem-������, ��������� ��������, ���������� ��������� [SaveCommand], ��� ������������ �������� ���������
        /// </summary>
        /// <param name="item"></param>
        /// <param name="connection"></param>
        /// <param name="configureSetCommand"></param>
        public void SaveBaseItem(BaseEntity item, SqlConnection connection, ConfigureSetCommand configureSetCommand = null)
        {
            if (!item.IsModified) return;

            if (item.State == EState.None) return;

            var sqlSetCmd = CreateSaveStoredProcedure(item, connection);

            if (configureSetCommand != null)
                configureSetCommand(sqlSetCmd);

            SaveBaseItem(sqlSetCmd);

            var parameterId = sqlSetCmd.Parameters["@p_ID"];
            if (parameterId.Direction == ParameterDirection.InputOutput)
                item.ID = (int)parameterId.Value;
        }

        /// <summary>
        /// ����� �������� SqlCommand � ����������� ���������� ��� ���������� ������� ����� ���������
        /// </summary>
        /// <param name="sqlConn">����������� � ��</param>
        /// <param name="item">����������� ������</param>
        /// <returns></returns>
        public SqlCommand CreateSaveStoredProcedure(BaseEntity item, SqlConnection sqlConn)
        {
            var sqlCommand = sqlConn.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;

            var type = item.GetType();
            var saveCommand = type.GetCustomAttribute<SaveCommandAttribute>();

            if (saveCommand == null)
                throw new GeneratingStoredProcedureNotSupportedException(
                    string.Format("������ '{0}' �� ������������ ��������� ������� ����������. ����������� [SaveCommandAttribute] ��������.", type));

            sqlCommand.CommandText = saveCommand.Name;
            if (string.IsNullOrEmpty(sqlCommand.CommandText) && !saveCommand.UseEmptyCommandName)
                sqlCommand.CommandText = $"xp_Save{type.Name}";

            sqlCommand.CommandTimeout = saveCommand.Timeout;

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
                sqlCommand.Parameters.Add(sqlParameter);
            }

            return sqlCommand;
        }

        private static bool CheckValueIsNull(object value, SaveParameterAttribute parameter)
        {
            return (value == null && parameter.NullValue == null) ||
                   (value != null && value.Equals(parameter.NullValue)) ||
                   (value is DateTime && DateTime.MinValue == (DateTime)value) ||
                   (value is Decimal && parameter.NullValue != null && value.Equals(Convert.ToDecimal(parameter.NullValue)));
        }

        private static bool CheckValueIsIdentifier(PropertyInfo property, BaseEntity item)
        {
            return property.Name == nameof(item.ID) &&
                property.PropertyType == typeof(int);
        }

        public void SaveCollection<T>(IEntityCollection<T> collection, Action<T> itemSaveAction)
            where T : BaseEntity
        {
            if (!collection.IsModified) return;

            foreach (var item in collection)
                itemSaveAction(item);
        }

        private SqlConnection CreateSqlConnection()
        {
            //return new SqlConnection(@"packet size=4096; data source=SERV72\TAMUZ; persist security info=True; initial catalog=JDB_ERP_1; Integrated Security = SSPI;");
            return new SqlConnection(_connectionString);
        }
    }
}