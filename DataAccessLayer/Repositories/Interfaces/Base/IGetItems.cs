using Entities.Base;
using Entities.Base.Utils.Interface;

namespace DataAccessLayer.Repositories.Interfaces.Base
{
    /// <summary>
    /// Интерфейс для получения коллекции объектов типа <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">Тип сохраняемого объекта.</typeparam>
    public interface IGetItems<T> where T : BaseEntity
    {
        /// <summary>
        /// Получение коллекции объектов.
        /// </summary>
        /// <param name="parametersContainer">Контейнер с параметрами хранимой процедуры.</param>
        /// <returns></returns>
        EntityCollection<T> GetItems(IParametersContainer parametersContainer);
    }
}
