using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using Entities;
using Entities.MenuUserHistory;

namespace DataAccessLayer.Mapping
{
    internal sealed class MapperContainer
    {
        public IDataMapper Data { get; private set; }
        public IMapper<SqlDataReaderWithSchema, UserRole> UserRole { get; private set; }
        public IMapper<SqlDataReaderWithSchema, RoleUser> RoleUser { get; private set; }
        public IMapper<SqlDataReaderWithSchema, MenuItem> MenuItem { get; private set; }
        public IMapper<SqlDataReaderWithSchema, UserSettings> UserSettings { get; private set; }

        public MapperContainer()
        {
            Data = new DataMapper();

            UserRole = new UserRoleMapper(Data);

            RoleUser = new RoleUserMapper(Data);

            MenuItem = new MenuItemMapper(Data);

            UserSettings = new UserSettingsMapper(Data);
        }
    }
}
