using Entities.Base;

namespace DataAccessLayer.Repositories.Interfaces.Base
{
    /// <summary>
    /// Интерфейс для сохранения объекта типа <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">Тип сохраняемого объекта.</typeparam>
    public interface ISave<T> where T : BaseEntity
    {
        /// <summary>
        /// Сохранение объекта.
        /// </summary>
        /// <param name="item">Сохраняемый объект.</param>
        void SaveItem(T item);
    }
}
