using Entities.Base;

namespace DataAccessLayer.Repositories.Interfaces.Base
{
    /// <summary>
    /// Интерфейс для получения объекта типа <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">Тип получаемого объекта.</typeparam>
    public interface IGetItemByID<T> where T : BaseEntity
    {
        /// <summary>
        /// Получение объекта по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <returns></returns>
        T GetItemByID(int id);
    }
}
