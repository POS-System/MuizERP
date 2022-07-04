using DataAccessLayer.Parameters;
using Entities.Base;
using System.Collections.ObjectModel;

namespace DataAccessLayer.Interfaces.Base
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
        /// <param name="parameters">Контейнер с параметрами хранимой процедуры.</param>
        /// <returns></returns>
        ObservableCollection<T> GetItems(IParametersContainer parameters);
    }
}
