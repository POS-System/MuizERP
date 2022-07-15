using DataAccessLayer.DataReaders;
using Entities.Base;
using System;

namespace DataAccessLayer.Mapping.Interface
{
    internal interface IDataMapper
    {
        /// <summary>
        /// Метод базового мэппинга.
        /// </summary>
        /// <param name="drd"></param>
        /// <param name="item"></param>
        /// <param name="fieldNameAction"></param>
        void Map(SqlDataReaderWithSchema drd, BaseEntity item, Action<string> fieldNameAction = null);
    }
}
