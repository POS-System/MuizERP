using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using Entities;
using Entities.Base;
using Entities.Base.Attributes;

namespace DataAccessLayer.Mapping
{
    internal sealed class MapperContainer
    {
        public IMapper<SqlDataReaderWithSchema, BaseEntity> Base { get; private set; }
        public IMapper<SqlDataReaderWithSchema, UserRole> UserRole { get; private set; }
        public IMapper<SqlDataReaderWithSchema, RoleUser> RoleUser { get; private set; }
        public IMapper<SqlDataReaderWithSchema, MenuItem> MenuItem { get; private set; }

        public MapperContainer()
        {
            Base = new BaseMapper();

            UserRole = new UserRoleMapper(Base, Base, new AttributeNameSetter<LoadParameterAttribute, UserRole>());

            RoleUser = new RoleUserMapper(Base, Base, new AttributeNameSetter<LoadParameterAttribute, RoleUser>());

            MenuItem = new MenuItemMapper(Base);
        }


    }
}
