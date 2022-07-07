using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using Entities;
using Entities.Base.Attributes;
using Entities.Base.Utils;
using System;

namespace DataAccessLayer.Mapping
{
    internal sealed class RoleUserMapper : IMapper<SqlDataReaderWithSchema, RoleUser>
    {
        private readonly IMapper<SqlDataReaderWithSchema, RoleUser> _baseMapper;
        private readonly IMapper<SqlDataReaderWithSchema, User> _userMapper;

        public RoleUserMapper(
            IMapper<SqlDataReaderWithSchema, RoleUser> baseMapper,
            IMapper<SqlDataReaderWithSchema, User> userMapper)
        {
            _baseMapper = baseMapper;
            _userMapper = userMapper;
        }

        public void Map(SqlDataReaderWithSchema drd, RoleUser item)
        {
            var type = typeof(RoleUser);
            var loadParameter = type.GetProperty(nameof(item.TimeStamp)).GetCustomAttribute<LoadParameterAttribute>();
            if (loadParameter != null)
            {
                loadParameter.Name = $"{type.Name}{nameof(item.TimeStamp)}";
            }

            _baseMapper.Map(drd, item);
            _userMapper.Map(drd, item.User);
        }
    }
}
