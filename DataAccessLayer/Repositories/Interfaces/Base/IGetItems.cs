﻿using Entities.Base;
using Entities.Base.Parameters;
using System.Collections.ObjectModel;

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
        ObservableCollection<T> GetItems(IParametersContainer parametersContainer);
    }
}