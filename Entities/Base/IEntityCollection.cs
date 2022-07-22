using System.Collections;
using System.Collections.Generic;

namespace Entities.Base
{
    /// <summary>
    /// Интерфейс для работы с состояниями объектов коллекции.
    /// </summary>
    public interface IEntityCollection : IEnumerable
    {
        /// <summary>
        /// Признак изменения элементов коллекции.
        /// </summary>
        bool IsModified { get; }

        /// <summary>
        /// Фиксирует текущие значения элементов коллекции.
        /// </summary>
        void Fix();

        /// <summary>
        /// Устанавливает состояние объектов коллекции в None.
        /// </summary>
        void ResetState();
    }

    /// <summary>
    /// Интерфейс коллекция объектов, типизированная наследниками <see cref="BaseEntity"/>.
    /// </summary>
    /// <typeparam name="T">Наследник <see cref="BaseEntity"/>.</typeparam>
    public interface IEntityCollection<T> : IEntityCollection, IEnumerable<T>
        where T : BaseEntity
    {
        /// <summary>
        /// Добавляет в коллекцию несколько объектов.
        /// </summary>
        /// <param name="items"></param>
        void AddRange(IEnumerable<T> items);

        /// <summary>
        /// Удаляет объект.
        /// </summary>
        /// <param name="item"></param>
        void RemoveItem(T item);

        /// <summary>
        /// Удаляет объекты, переданные списком.
        /// </summary>
        /// <param name="items"></param>
        void RemoveRange(IEnumerable<T> items);

        /// <summary>
        /// Возвращает объекты, не помеченные на удаление.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetActualItems();

        /// <summary>
        /// Возвращает объекты, сохраняемые в БД.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetSavableItems();
    }
}
