using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using Entities;
using System;

namespace DataAccessLayer.Mapping
{
    internal sealed class MenuItemMapper : IMapper<SqlDataReaderWithSchema, MenuItem>
    {
        private readonly IDataMapper _dataMapper;

        public MenuItemMapper(
            IDataMapper dataMapper)
        {
            _dataMapper = dataMapper;
        }

        public void Map(SqlDataReaderWithSchema drd, MenuItem item)
        {
            var typeName = drd["Entity"];
            if (typeName != null)
                item.EntityType = Type.GetType($"Entities.{typeName}, Entities");

            _dataMapper.Map(drd, item);
        }
    }
}
