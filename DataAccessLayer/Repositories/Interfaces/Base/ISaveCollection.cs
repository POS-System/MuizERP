using Entities.Base;

namespace DataAccessLayer.Repositories.Interfaces.Base
{
    /// <summary>
    /// Интерфейс для быстрого сохранения коллекции объектов.
    /// </summary>
    /// <typeparam name="T">Тип объекта в коллекции.</typeparam>
    public interface ISaveCollection<T> where T : BaseEntity
    {
        /// <summary>
        /// Сохраняет коллекцию объектов.
        /// </summary>
        /// <param name="collection"></param>
        void SaveCollection(IEntityCollection<T> collection);
    }
}
