using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using DataAccessLayer.DataReaders;
using DataAccessLayer.Delegates;
using Entities.Base.Attributes;
using Entities.Exceptions.InnerApplicationExceptions;
using Entities.Utils;
using IsolationLevel = System.Transactions.IsolationLevel;
using Entities.Exceptions.LogicExceptions;

namespace DataAccessLayer
{
    /// <summary>
    /// ����� ����������� ��� ������ � ��
    /// </summary>
    internal sealed class DataBaseDAL
    {
        [ThreadStatic]
        private static SqlConnection _connectionInTransaction;

        public DataBaseDAL()
        {
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
        /// <param name="source">�������� �����������.</param>
        public void ReadCollectionWithSchema(InitSqlCommand init, ReadDataReaderWithSchema read)
        {
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
        public void ReadCollectionWithSchema(string commandName, ReadDataReaderWithSchema read)
        {
            ReadCollectionWithSchema(sqlCmd => { sqlCmd.CommandText = commandName; }, read);
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
                connection.Open();
                action(connection);
            }
        }

        private void DoInConnectionSessionInsideTransaction(Action<SqlConnection> action)
        {
            action(_connectionInTransaction);
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
        public void SetBaseItem(SqlCommand sqlSetCmd)
        {
            sqlSetCmd.ExecuteNonQuery();
        }

        /// <summary>
        /// ��������� BaseItem-������, ��������� ������� prepare ��� �������� ���������� �������� ���������
        /// </summary>
        /// <param name="item"></param>
        /// <param name="connection"></param>
        /// <param name="prepare"></param>
        public void SetBaseItem(SqlConnection connection, PrepareSetCommand prepare)
        {
            var sqlSetCmd = prepare(connection);
            SetBaseItem(sqlSetCmd);
        }

        /// <summary>
        /// ��������� BaseItem-������, ��������� ��������, ���������� ��������� [SaveCommand], ��� ������������ �������� ���������
        /// </summary>
        /// <param name="item"></param>
        /// <param name="connection"></param>
        /// <param name="configureSetCommand"></param>
        public int SetBaseItem(object item, SqlConnection connection, ConfigureSetCommand configureSetCommand)
        {
            var sqlSetCmd = CreateSetStoredProcedure(item, connection);

            if (configureSetCommand != null)
                configureSetCommand(sqlSetCmd);

            SetBaseItem(sqlSetCmd);

            // � ���������������� �����
            // ��������� �� ���������� ID, � ����������� ��� � ����������� �������
            var itemId = 0;

            var parameterId = sqlSetCmd.Parameters["@p_ID"];
            if (parameterId.Direction == ParameterDirection.InputOutput)
                itemId = (int)parameterId.Value;

            return itemId;
        }

        /// <summary>
        /// ����� �������� SqlCommand � ����������� ���������� ��� ���������� ������� ����� ���������
        /// </summary>
        /// <param name="sqlConn">����������� � ��</param>
        /// <param name="item">����������� ������</param>
        /// <returns></returns>
        public SqlCommand CreateSetStoredProcedure(object item, SqlConnection sqlConn)
        {
            var sqlCommand = sqlConn.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;

            var type = item.GetType();
            var saveCommand = type.GetCustomAttribute<SaveCommandAttribute>();

            if (saveCommand == null)
                throw new GeneratingStoredProcedureNotSupportedException(string.Format("������ '{0}' �� ������������ ��������� ������� ����������." +
                                        "����������� [SaveCommandAttribute] ��������.", type));
            if (string.IsNullOrEmpty(saveCommand.Name) && !saveCommand.UseEmptyCommandName)
                throw new GeneratingStoredProcedureNotSupportedException(string.Format("������ '{0}' �� ������������ ��������� ������� ����������." +
                                        "����������� �������� 'Name' ��������� [SaveCommandAttribute].", type));

            sqlCommand.CommandText = saveCommand.Name;
            sqlCommand.CommandTimeout = saveCommand.Timeout;

            var props = type.GetProperties();
            foreach (var property in props)
            {
                var parameter = property.GetCustomAttribute<SaveParameterAttribute>();
                // ���� �������� �� ��������� � ����������, ���������� ���
                if (parameter == null) continue;

                // �������� �������� ���������
                var parameterName = parameter.Name;

                // ���� �� ������, ���������� �������� �������� � ��������� @p_
                if (string.IsNullOrEmpty(parameterName))
                    parameterName = $"@p_{property.Name}";

                // ����������� ��������
                var value = property.GetValue(item, null);

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

                if (parameter.Size > 0) sqlParameter.Size = parameter.Size;
                sqlParameter.Direction = parameter.Direction;

                if (sqlParameter.ParameterName == "@p_ID")
                    sqlParameter.Direction = ParameterDirection.InputOutput;

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

        public void SaveCollection<T>(IEnumerable<T> collection, Action<T> itemSaveAction)
        {
            foreach (var item in collection)
                itemSaveAction(item);
        }

        private SqlConnection CreateSqlConnection()
        {
            return new SqlConnection(@"packet size=4096; data source=SERV72\TAMUZ; persist security info=True; initial catalog=JDB_ERP_1; Integrated Security = SSPI;");
            
        }
    }
}