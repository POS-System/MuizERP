using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using Entities;
using System;

namespace DataAccessLayer.Mapping
{
    internal sealed class MenuItemMapper : IMapper<SqlDataReaderWithSchema, MenuItem>
    {
        private readonly IMapper<SqlDataReaderWithSchema, MenuItem> _baseMapper;

        public MenuItemMapper(
            IMapper<SqlDataReaderWithSchema, MenuItem> baseMapper)
        {
            _baseMapper = baseMapper;
        }

        public void Map(SqlDataReaderWithSchema drd, MenuItem item)
        {
            var typeName = drd["Entity"];
            if (typeName != null)
                item.EntityType = Type.GetType($"Entities.{typeName}, Entities");

            _baseMapper.Map(drd, item);
        }
    }
}
