using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using Entities;
using Entities.Base.Attributes;

namespace DataAccessLayer.Mapping
{
    internal sealed class RoleUserMapper : IMapper<SqlDataReaderWithSchema, RoleUser>
    {
        private readonly IMapper<SqlDataReaderWithSchema, RoleUser> _baseMapper;
        private readonly IMapper<SqlDataReaderWithSchema, User> _userMapper;

        private readonly IAttributeSetter<LoadParameterAttribute, RoleUser> _loadAttributeNameSetter;

        public RoleUserMapper(
            IMapper<SqlDataReaderWithSchema, RoleUser> baseMapper,
            IMapper<SqlDataReaderWithSchema, User> userMapper,
            IAttributeSetter<LoadParameterAttribute, RoleUser> loadAttributeNameSetter)
        {
            _baseMapper = baseMapper;
            _userMapper = userMapper;

            _loadAttributeNameSetter = loadAttributeNameSetter;
        }

        public void Map(SqlDataReaderWithSchema drd, RoleUser item)
        {
            var propertyName = nameof(item.TimeStamp);
            _loadAttributeNameSetter.Set(propertyName, $"{nameof(RoleUser)}{propertyName}");

            _baseMapper.Map(drd, item);
            _userMapper.Map(drd, item.User);
        }
    }
}
