using DataAccessLayer.Mapping;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities;
using Entities.Base.Attributes;

namespace DataAccessLayer
{
    public class DALContainer
    {
        private ISampleEntityRepository _sampleEntityRepository;
        private ISampleEntityDetailsRepository _sampleEntityDetailsRepository;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private IUserRoleRepository _userRoleRepository;
        private IRoleUserRepository _roleUserRepository;
        private ICompanyRepository _companyRepository;
        private IMainMenuRepository _mainMenuRepository;

        public ISampleEntityRepository SampleEntityRepository
        {
            get { return _sampleEntityRepository; }
        }

        public ISampleEntityDetailsRepository SampleEntityDetailsRepository
        {
            get { return _sampleEntityDetailsRepository; }
        }

        public IUserRepository UserRepository
        {
            get { return _userRepository; }
        }

        public ICompanyRepository CompanyRepository
        {
            get { return _companyRepository; }
        }

        public IRoleRepository RoleRepository
        {
            get { return _roleRepository; }
        }

        public IUserRoleRepository UserRoleRepository
        {
            get { return _userRoleRepository; }
        }

        public IRoleUserRepository RoleUserRepository
        {
            get { return _roleUserRepository; }
        }

        public IMainMenuRepository MainMenuRepository
        {
            get { return _mainMenuRepository; }
        }

        public DALContainer(string connectionString)
        {
            var sqlExceptionConverter = new SqlExceptionToLogicExceptionConverter();

            var dataBaseRepository = new DataBaseRepository(connectionString, sqlExceptionConverter);

            var baseMapper = new BaseMapper();
            var userRoleMapper = new UserRoleMapper(
                baseMapper,
                baseMapper,
                new AttributeNameSetter<LoadParameterAttribute, UserRole>());
            var roleUserMapper = new RoleUserMapper(
                baseMapper,
                baseMapper,
                new AttributeNameSetter<LoadParameterAttribute, RoleUser>());
            var menuItemMapper = new MenuItemMapper(baseMapper);

            _sampleEntityDetailsRepository = new SampleEntityDetailsRepository(dataBaseRepository, baseMapper);
            _sampleEntityRepository = new SampleEntityRepository(dataBaseRepository, _sampleEntityDetailsRepository, baseMapper);
            _companyRepository = new CompanyRepository(dataBaseRepository, baseMapper);     
            
            _userRoleRepository = new UserRoleRepository(dataBaseRepository, userRoleMapper);
            _roleUserRepository = new RoleUserRepository(dataBaseRepository, roleUserMapper);

            _roleRepository = new RoleRepository(dataBaseRepository, _roleUserRepository, baseMapper);
            _userRepository = new UserRepository(dataBaseRepository, _userRoleRepository, baseMapper);

            _mainMenuRepository = new MainMenuRepository(dataBaseRepository, menuItemMapper);
        }
    }
}
