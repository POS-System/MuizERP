using DataAccessLayer.Mapping;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;

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

        public DALContainer(string connectionString)
        {
            var sqlExceptionConverter = new SqlExceptionToLogicExceptionConverter();

            var dataBaseRepository = new DataBaseRepository(connectionString, sqlExceptionConverter);

            var mappers = new MapperContainer();

            SampleEntityDetailsRepository = new SampleEntityDetailsRepository(dataBaseRepository, mappers.Base);
            SampleEntityRepository = new SampleEntityRepository(dataBaseRepository, SampleEntityDetailsRepository, mappers.Base);
            CompanyRepository = new CompanyRepository(dataBaseRepository, mappers.Base);     
            
            UserRoleRepository = new UserRoleRepository(dataBaseRepository, mappers.UserRole);
            RoleUserRepository = new RoleUserRepository(dataBaseRepository, mappers.RoleUser);

            RoleRepository = new RoleRepository(dataBaseRepository, RoleUserRepository, mappers.Base);
            UserRepository = new UserRepository(dataBaseRepository, UserRoleRepository, mappers.Base);

            MainMenuRepository = new MainMenuRepository(dataBaseRepository, mappers.MenuItem);
        }
    }
}
