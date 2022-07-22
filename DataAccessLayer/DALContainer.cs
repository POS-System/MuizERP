using DataAccessLayer.Factories;
using DataAccessLayer.Mapping;
using DataAccessLayer.Providers;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities.Base.Providers;

namespace DataAccessLayer
{
    public class DALContainer
    {
        public ISampleEntityRepository SampleEntityRepository { get; private set; }
        public ISampleEntityDetailsRepository SampleEntityDetailsRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }
        public ICompanyRepository CompanyRepository { get; private set; }
        public IRoleRepository RoleRepository { get; private set; }
        public IUserRoleRepository UserRoleRepository { get; private set; }
        public IRoleUserRepository RoleUserRepository { get; private set; }
        public IMainMenuRepository MainMenuRepository { get; private set; }
        public IUserSettingsRepository UserSettingsRepository { get; private set; }
        public IUserMainMenuFavoritesRepository UserMainMenuFavoritesRepository { get; private set; }
        public IUserMainMenuHistoryRepository UserMainMenuHistoryRepository { get; private set; }

        public DALContainer(string connectionString)
        {
            var dataTableFactory = new DataTableFactory(
                new TypeBySqlTypeNameProvider(), new NameByPropertyProvider());

            var dataRepository = new DataRepository(
                connectionString,
                sqlExcepionConverter: new SqlExceptionToLogicExceptionConverter(),
                dataTableFactory);

            var mappers = new MapperContainer();

            SampleEntityDetailsRepository = new SampleEntityDetailsRepository(dataRepository, mappers.Data);
            SampleEntityRepository = new SampleEntityRepository(dataRepository, SampleEntityDetailsRepository, mappers.Data);
            CompanyRepository = new CompanyRepository(dataRepository, mappers.Data);     
            
            UserRoleRepository = new UserRoleRepository(dataRepository, mappers.UserRole);
            RoleUserRepository = new RoleUserRepository(dataRepository, mappers.RoleUser);

            RoleRepository = new RoleRepository(dataRepository, RoleUserRepository, mappers.Data);
            UserMainMenuFavoritesRepository = new UserMainMenuFavoritesRepository(dataRepository, mappers.UserMenuItem);
            UserMainMenuHistoryRepository = new UserMainMenuHistoryRepository(dataRepository, mappers.UserMenuItem);

            UserRepository = new UserRepository(
                dataRepository,
                UserRoleRepository,
                UserMainMenuFavoritesRepository,
                UserMainMenuHistoryRepository,
                mappers.Data);

            MainMenuRepository = new MainMenuRepository(dataRepository, mappers.Data);

            UserSettingsRepository = new UserSettingsRepository(dataRepository, mappers.UserSettings);
        }
    }
}
