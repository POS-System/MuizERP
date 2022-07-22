using DataAccessLayer.DataReaders;
using DataAccessLayer.Delegates;
using Entities.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DataAccessLayer.Repositories.Interfaces
{
    /// <summary>
    ///  ������� ������ ��� ������ � ��
    /// </summary>
    internal interface IDataRepository
    {
        #region Wrappers

        /// <summary>
        /// ��������� ��������� � ����������� ����������, ������������ �������� inside
        /// </summary>
        /// <param name="inside"></param>
        void DoInTransaction(InsideTransaction inside);

        /// <summary>
        /// ��������� ������� � ��������� ���������� � ��
        /// </summary>
        /// <param name="action">�������� ����������� � ��������� ���������� ��</param>
        void DoInConnectionSession(Action<SqlConnection> action);

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
        T GetSingleItem<T>(InitSqlCommand init, Func<SqlDataReaderWithSchema, T> read);

        /// <summary>
        /// ���������� sql ������� � ������ ������ ������ �� ���������� � ������.
        /// ���� ��������� �� �������� �� ����� ������, ���������� default.
        /// ���� ��������� �������� ����� ����� ������, ������������ ������ <see cref="SqlDataReaderExpectsOnlyOneRecordException"/>.
        /// </summary>
        /// <typeparam name="T">��� �������, � ������� ����������� ���� �� ����������.</typeparam>
        /// <param name="init">�������, ���������������� sql �������.</param>
        /// <param name="read">�������, ����������� ������ ������ �� ����������.</param>
        /// <returns>����������� �� ���������� ������ ��� default.</returns>
        T GetSingleItemOrDefault<T>(InitSqlCommand init, Func<SqlDataReaderWithSchema, T> read);

        /// <summary>
        /// ���������� sql ������� � ������ ������ ������ �� ���������� � ������.
        /// ���� ��������� �� �������� �� ����� ������, ������������ ���������� ������.
        /// ���� ��������� �������� ����� ����� ������, ������������ ������ <see cref="SqlDataReaderExpectsOnlyOneRecordException"/>.
        /// </summary>
        /// <typeparam name="T">��� �������, � ������� ����������� ���� �� ����������.</typeparam>
        /// <param name="init">�������, ���������������� sql �������.</param>
        /// <param name="read">�������, ����������� ������ ������ �� ����������.</param>
        /// <returns>����������� �� ���������� ������.</returns>
        T GetSingleItemOrThrowLogicException<T>(InitSqlCommand init, Func<SqlDataReaderWithSchema, T> read);

        /// <summary>
        /// ���������� sql ������� � ������ ������ ������ �� ����������.
        /// </summary>
        /// <param name="init"></param>
        /// <param name="read"></param>
        void ReadFirstItem(InitSqlCommand init, ReadDataReaderWithSchema read);

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
        /// <returns>�������������� ���������.</returns>
        TCollection GetCollectionWithSchema<TCollection, TItem>(
            Func<TCollection> createCollection,
            InitSqlCommand init,
            Action<SqlDataReaderWithSchema, TCollection> read)
            where TCollection : IEnumerable<TItem>
            where TItem : BaseEntity;

        /// <summary>
        /// ���������� sql ������� � ������ ������� � ���������.
        /// </summary>
        /// <typeparam name="TCollection">��� ���������.</typeparam>
        /// <typeparam name="TItem">��� �������� ���������.</typeparam>
        /// <param name="createCollection">������� ��������� ��������� ���������.</param>
        /// <param name="commandName">�������� �������� ��������� ��� ���������.</param>
        /// <param name="read">�������, ����������� ������ ������ �� ���������� � ������. � ����������� ������ � ���������.</param>
        /// <param name="source">�������� �����������.</param>
        /// <returns>�������������� ���������.</returns>
        TCollection GetCollectionWithSchema<TCollection, TItem>(
            Func<TCollection> createCollection,
            string commandName,
            Action<SqlDataReaderWithSchema, TCollection> read)
            where TCollection : IEnumerable<TItem>
            where TItem : BaseEntity;

        /// <summary>
        /// ���������� sql ������� � ������ ������� � ���������.
        /// </summary>
        /// <param name="init">�������, ���������������� sql �������.</param>
        /// <param name="read">�������, ����������� ������ ������ �� ���������� � ������. � ����������� ������ � ���������.</param>
        void ReadCollectionWithSchema(InitSqlCommand init, ReadDataReaderWithSchema read);

        /// <summary>
        /// ���������� sql ������� � ������ ������� � ���������.
        /// </summary>
        /// <param name="commandName">�������� �������� ��������� ��� ���������.</param>
        /// <param name="read">�������, ����������� ������ ������ �� ���������� � ������. � ����������� ������ � ���������.</param>
        void ReadCollectionWithSchema(string commandName, ReadDataReaderWithSchema read);

        #endregion

        #region Save item

        /// <summary>
        /// ��������� BaseItem-������, ��������� ��������, ���������� ��������� [SaveCommand], ��� ������������ �������� ���������
        /// </summary>
        /// <param name="item"></param>
        /// <param name="connection"></param>
        /// <param name="configureSetCommand"></param>
        /// <param name="getOutputValues"></param>
        void SaveBaseItem(BaseEntity item, SqlConnection connection, ConfigureSqlCommand configureSetCommand = null, GetValuesFromOutputParameters getOutputValues = null);

        /// <summary>
        /// ��������� BaseItem-������, ��������� ������� prepare ��� �������� ���������� �������� ���������
        /// </summary>
        /// <param name="item"></param>
        /// <param name="connection"></param>
        /// <param name="prepare"></param>
        /// <param name="getOutputValues"></param>
        void SaveBaseItem(BaseEntity item, SqlConnection connection, PrepareSqlCommand prepare, GetValuesFromOutputParameters getOutputValues = null);

        /// <summary>
        /// ��������� BaseItem-������, ��������� ���������� ������ �������� ���������. �� ���������� ���������� ������� ������ ItemStates.None.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="sqlSetCmd"></param>
        /// <param name="getOutputValues"></param>
        void SaveBaseItem(BaseEntity item, SqlCommand sqlSetCmd, GetValuesFromOutputParameters getOutputValues = null);
        
        /// <summary>
        /// ��������� BaseItem-������, ��������� ��������, ���������� ��������� [SaveCommand], ��� ������������ �������� ���������
        /// </summary>
        void SaveBaseItem(BaseEntity item, SqlConnection connection, int editUserId, ConfigureSqlCommand configureSetCommand, GetValuesFromOutputParameters getOutputValues = null);

        /// <summary>
        /// ��������� BaseItem-������, ��������� ��������, ���������� ��������� [SaveCommand], ��� ������������ �������� ���������
        /// </summary>
        /// <param name="item"></param>
        /// <param name="connection"></param>
        /// <param name="editUserId"></param>
        /// <param name="getOutputValues"></param>
        void SaveBaseItem(BaseEntity item, SqlConnection connection, int editUserId, GetValuesFromOutputParameters getOutputValues = null);

        #endregion

        #region Save collection

        /// <summary>
        /// ��������� ����� ���������� ��������� ���������. �� ��������� ���������� ������� ������� ����� ������� ��������� �� ��������� ������.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="itemSaveAction"></param>
        void SaveCollection<T>(EntityCollection<T> collection, Action<T> itemSaveAction)
            where T : BaseEntity;

        /// <summary>
        /// ��������� ��������� �������� � ������� DataTable.
        /// </summary>
        /// <param name="collection">��������� ��������</param>
        /// <param name="configureSqlCommand">������� ��� ���������������� sql-�������.</param>
        void SaveCollectionWithDataTable(IEntityCollection collection, ConfigureSqlCommand configureSqlCommand = null);

        #endregion
    }
}