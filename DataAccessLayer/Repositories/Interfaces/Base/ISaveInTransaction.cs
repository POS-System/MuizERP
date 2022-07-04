using Entities.Base;
using System.Data.SqlClient;

namespace DataAccessLayer.Repositories.Interfaces.Base
{
    /// <summary>
    /// Интерфейс для сохранения объекта типа <see cref="T"/> в транзакции.
    /// </summary>
    /// <typeparam name="T">Тип сохраняемого объекта.</typeparam>
    public interface ISaveInTransaction<T> where T : BaseEntity
    {
        /// <summary>
        /// Сохранение объекта в транзакции.
        /// </summary>
        /// <param name="item">Сохраняемый объект.</param>
        /// <param name="conn">Соединение транзакции.</param>
        void SaveItem(T item, SqlConnection conn);
    }
}
