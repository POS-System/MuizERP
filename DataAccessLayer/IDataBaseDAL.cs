using DataAccessLayer.DataReaders;
using DataAccessLayer.Delegates;
using Entities.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace DataAccessLayer.Interfaces.Base
{
    /// <summary>
    ///  Базовые методы для работы с БД
    /// </summary>
    internal interface IDataBaseDataRepository
    {
        /// <summary>
        /// Выполнение sql команды и чтение записей в коллекцию.
        /// </summary>
        /// <typeparam name="TCollection">Тип коллекции.</typeparam>
        /// <typeparam name="TItem">Тип элемента коллекции.</typeparam>
        /// <param name="createCollection">Делегат создающий экземпляр коллекции.</param>
        /// <param name="init">Делегат, инициализирующий sql команду.</param>
        /// <param name="read">Делегат, выполняющий чтение данных из датаридера в объект. И добавляющий объект в коллекцию.</param>
        /// <returns>Результирующая коллекция.</returns>
        TCollection GetCollectionWithSchema<TCollection, TItem>(
            Func<TCollection> createCollection,
            InitSqlCommand init,
            Action<SqlDataReaderWithSchema, TCollection> read)
            where TCollection : IEnumerable<TItem>
            where TItem : BaseEntity;


        /// <summary>
        /// Выполнение sql команды и чтение записей в коллекцию.
        /// </summary>
        /// <typeparam name="TCollection">Тип коллекции.</typeparam>
        /// <typeparam name="TItem">Тип элемента коллекции.</typeparam>
        /// <param name="createCollection">Делегат создающий экземпляр коллекции.</param>
        /// <param name="commandName">Название хранимой процедуры для выолнения.</param>
        /// <param name="read">Делегат, выполняющий чтение данных из датаридера в объект. И добавляющий объект в коллекцию.</param>
        /// <param name="source">Источник подключения.</param>
        /// <returns>Результирующая коллекция.</returns>
        TCollection GetCollectionWithSchema<TCollection, TItem>(
            Func<TCollection> createCollection,
            string commandName,
            Action<SqlDataReaderWithSchema, TCollection> read)
            where TCollection : IEnumerable<TItem>
            where TItem : BaseEntity;

        /// <summary>
        /// Выполнение sql команды и чтение записей в коллекцию.
        /// </summary>
        /// <param name="init">Делегат, инициализирующий sql команду.</param>
        /// <param name="read">Делегат, выполняющий чтение данных из датаридера в объект. И добавляющий объект в коллекцию.</param>
        void ReadCollectionWithSchema(InitSqlCommand init, ReadDataReaderWithSchema read);

        /// <summary>
        /// Выполнение sql команды и чтение записей в коллекцию.
        /// </summary>
        /// <param name="commandName">Название хранимой процедуры для выолнения.</param>
        /// <param name="read">Делегат, выполняющий чтение данных из датаридера в объект. И добавляющий объект в коллекцию.</param>
        void ReadCollectionWithSchema(string commandName, ReadDataReaderWithSchema read);

        /// <summary>
        /// Выполнение sql команды и чтение первой записи из датаридера в объект.
        /// Если датаридер не содержит ни одной записи, генерируется ошибка <see cref="SqlDataReaderExpectsRecordException"/>.
        /// Если датаридер содержит более одной записи, генерируется ошибка <see cref="SqlDataReaderExpectsOnlyOneRecordException"/>.
        /// </summary>
        /// <typeparam name="T">Тип объекта, в который заполняются поля из датаридера.</typeparam>
        /// <param name="init">Делегат, инициализирующий sql команду.</param>
        /// <param name="read">Делегат, выполняющий чтение данных из датаридера.</param>
        /// <returns>Заполненный из датаридера объект или NULL.</returns>
        T GetSingleItem<T>(InitSqlCommand init, Func<SqlDataReaderWithSchema, T> read);


        /// <summary>
        /// Выполнение sql команды и чтение первой записи из датаридера в объект.
        /// Если датаридер не содержит ни одной записи, возвращает default.
        /// Если датаридер содержит более одной записи, генерируется ошибка <see cref="SqlDataReaderExpectsOnlyOneRecordException"/>.
        /// </summary>
        /// <typeparam name="T">Тип объекта, в который заполняются поля из датаридера.</typeparam>
        /// <param name="init">Делегат, инициализирующий sql команду.</param>
        /// <param name="read">Делегат, выполняющий чтение данных из датаридера.</param>
        /// <returns>Заполненный из датаридера объект или default.</returns>
        T GetSingleItemOrDefault<T>(InitSqlCommand init, Func<SqlDataReaderWithSchema, T> read);


        /// <summary>
        /// Выполнение sql команды и чтение первой записи из датаридера в объект.
        /// Если датаридер не содержит ни одной записи, генерируется логическая ошибка.
        /// Если датаридер содержит более одной записи, генерируется ошибка <see cref="SqlDataReaderExpectsOnlyOneRecordException"/>.
        /// </summary>
        /// <typeparam name="T">Тип объекта, в который заполняются поля из датаридера.</typeparam>
        /// <param name="init">Делегат, инициализирующий sql команду.</param>
        /// <param name="read">Делегат, выполняющий чтение данных из датаридера.</param>
        /// <returns>Заполненный из датаридера объект.</returns>
        T GetSingleItemOrThrowLogicException<T>(InitSqlCommand init, Func<SqlDataReaderWithSchema, T> read);

        /// <summary>
        /// Выполнение sql команды и чтение первой записи из датаридера.
        /// </summary>
        /// <param name="init"></param>
        /// <param name="read"></param>
        void ReadFirstItem(InitSqlCommand init, ReadDataReaderWithSchema read);

        /// <summary>
        /// Управляет созданием и завершением транзакции, передаваемой делегату inside
        /// </summary>
        /// <param name="inside"></param>
        void DoInTransaction(InsideTransaction inside);

        /// <summary>
        /// Выполняет дествие в контексте соединения с БД
        /// </summary>
        /// <param name="action">Действие выполняемое в контексте соединения БД</param>
        void DoInConnectionSession(Action<SqlConnection> action);

        /// <summary>
        /// Сохраняет BaseItem-объект, используя свойства, помеченные атрибутом [SaveCommand], для формирования хранимой процедуры
        /// </summary>
        /// <param name="item"></param>
        /// <param name="connection"></param>
        /// <param name="configureSetCommand"></param>
        /// <param name="getOutputValues"></param>
        void SetBaseItem(BaseEntity item, SqlConnection connection, ConfigureSetCommand configureSetCommand = null, GetValuesFromOutputParameters getOutputValues = null);

        /// <summary>
        /// Сохраняет BaseItem-объект, используя делегат prepare для описания параметров хранимой процедуры
        /// </summary>
        /// <param name="item"></param>
        /// <param name="connection"></param>
        /// <param name="prepare"></param>
        /// <param name="getOutputValues"></param>
        void SetBaseItem(BaseEntity item, SqlConnection connection, PrepareSetCommand prepare, GetValuesFromOutputParameters getOutputValues = null);

        /// <summary>
        /// Сохраняет BaseItem-объект, используя переданный объект хранимой процедуры. По завершении выставляет объекту статус ItemStates.None.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="sqlSetCmd"></param>
        /// <param name="getOutputValues"></param>
        void SetBaseItem(BaseEntity item, SqlCommand sqlSetCmd, GetValuesFromOutputParameters getOutputValues = null);
        
        /// <summary>
        /// Сохраняет BaseItem-объект, используя свойства, помеченные атрибутом [SaveCommand], для формирования хранимой процедуры
        /// </summary>
        void SetBaseItem(BaseEntity item, SqlConnection connection, int editUserId, ConfigureSetCommand configureSetCommand, GetValuesFromOutputParameters getOutputValues = null);

        /// <summary>
        /// Сохраняет BaseItem-объект, используя свойства, помеченные атрибутом [SaveCommand], для формирования хранимой процедуры
        /// </summary>
        /// <param name="item"></param>
        /// <param name="connection"></param>
        /// <param name="editUserId"></param>
        /// <param name="getOutputValues"></param>
        void SetBaseItem(BaseEntity item, SqlConnection connection, int editUserId, GetValuesFromOutputParameters getOutputValues = null);

        /// <summary>
        /// Метод создания SqlCommand с заполнением параметров для сохранения объекта через рефлексию
        /// </summary>
        /// <param name="sqlConn">Покдлючение к БД</param>
        /// <param name="item">Сохраняемый объект</param>
        /// <param name="editUserId">Идентификатор пользователя, от чьего имени сохраняем информацию</param>
        /// <returns></returns>
        SqlCommand CreateSetStoredProcedure(BaseEntity item, SqlConnection sqlConn, int editUserId = 0);

        /// <summary>
        /// Шаблонный метод сохранения коллекции элементов. По окончании сохранения каждого элемена метод очищает коллекцию от удаленных членов.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="itemSaveAction"></param>
        void SaveCollection<T>(ObservableCollection<T> collection, Action<T> itemSaveAction) where T : BaseEntity;
    }
}