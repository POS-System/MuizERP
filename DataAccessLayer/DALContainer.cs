using DataAccessLayer.Mapping;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;

namespace DataAccessLayer
{
    public class DALContainer
    {
        private ISampleEntityRepository _sampleEntityRepository;
        private ISampleEntityDetailsRepository _sampleEntityDetailsRepository;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private ICompanyRepository _companyRepository;

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

        public DALContainer(string connectionString)
        {
            var sqlExceptionConverter = new SqlExceptionToLogicExceptionConverter();

            var dataBaseRepository = new DataBaseRepository(connectionString, sqlExceptionConverter);
            var baseMapper = new BaseMapper();
            var userRoleMapper = new UserRoleMapper(baseMapper);

            _sampleEntityDetailsRepository = new SampleEntityDetailsRepository(dataBaseRepository, baseMapper);
            _sampleEntityRepository = new SampleEntityRepository(dataBaseRepository, _sampleEntityDetailsRepository, baseMapper);
            _roleRepository = new RoleRepository(dataBaseRepository, baseMapper, userRoleMapper);
            _userRepository = new UserRepository(dataBaseRepository, _roleRepository, baseMapper);
            _companyRepository = new CompanyRepository(dataBaseRepository, baseMapper);
        }
    }
}
