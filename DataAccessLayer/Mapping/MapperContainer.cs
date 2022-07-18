using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using Entities;

namespace DataAccessLayer.Mapping
{
    internal sealed class MapperContainer
    {
        public IDataMapper Data { get; private set; }
        public IMapper<SqlDataReaderWithSchema, UserRole> UserRole { get; private set; }
        public IMapper<SqlDataReaderWithSchema, RoleUser> RoleUser { get; private set; }
        public IMapper<SqlDataReaderWithSchema, UserSettings> UserSettings { get; private set; }

        public MapperContainer()
        {
            Data = new DataMapper();

            UserRole = new UserRoleMapper(Data);

            RoleUser = new RoleUserMapper(Data);

            UserSettings = new UserSettingsMapper(Data);
        }
    }
}
