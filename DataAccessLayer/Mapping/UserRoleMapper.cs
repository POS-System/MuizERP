using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using Entities;
using Entities.Base.Attributes;
using Entities.Base.Utils;
using System;

namespace DataAccessLayer.Mapping
{
    internal sealed class UserRoleMapper : IMapper<SqlDataReaderWithSchema, UserRole>
    {
        private readonly IMapper<SqlDataReaderWithSchema, UserRole> _baseMapper;
        private readonly IMapper<SqlDataReaderWithSchema, Role> _roleMapper;

        public UserRoleMapper(
            IMapper<SqlDataReaderWithSchema, UserRole> baseMapper,
            IMapper<SqlDataReaderWithSchema, Role> roleMapper)
        {
            _baseMapper = baseMapper;
            _roleMapper = roleMapper;
        }

        public void Map(SqlDataReaderWithSchema drd, UserRole item)
        {
            var type = typeof(UserRole);
            var loadParameter = type.GetProperty(nameof(item.TimeStamp)).GetCustomAttribute<LoadParameterAttribute>();
            if (loadParameter != null)
            {
                loadParameter.Name = $"{type.Name}{nameof(item.TimeStamp)}";
            }

            _baseMapper.Map(drd, item);            
            _roleMapper.Map(drd, item.Role);
        }
    }
}
