using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using Entities;
using Entities.Base.Attributes;
using Entities.Base.Attributes.Interface;

namespace DataAccessLayer.Mapping
{
    internal sealed class UserRoleMapper : IMapper<SqlDataReaderWithSchema, UserRole>
    {
        private readonly IMapper<SqlDataReaderWithSchema, UserRole> _baseMapper;
        private readonly IMapper<SqlDataReaderWithSchema, Role> _roleMapper;

        private readonly IAttributeSetter<LoadParameterAttribute, UserRole> _loadAttributeNameSetter;

        public UserRoleMapper(
            IMapper<SqlDataReaderWithSchema, UserRole> baseMapper,
            IMapper<SqlDataReaderWithSchema, Role> roleMapper,
            IAttributeSetter<LoadParameterAttribute, UserRole> loadAttributeNameSetter)
        {
            _baseMapper = baseMapper;
            _roleMapper = roleMapper;

            _loadAttributeNameSetter = loadAttributeNameSetter;
        }

        public void Map(SqlDataReaderWithSchema drd, UserRole item)
        {
            var propertyName = nameof(item.TimeStamp);
            _loadAttributeNameSetter.Set(propertyName, $"{nameof(UserRole)}{propertyName}");

            _baseMapper.Map(drd, item);            
            _roleMapper.Map(drd, item.Role);
        }
    }
}
