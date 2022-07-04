using DataAccessLayer.Mapping;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Interfaces;

namespace DataAccessLayer
{
    public class DALContainer
    {
        private ISampleEntityRepository _sampleEntityRepository;
        private ISampleEntityDetailsRepository _sampleEntityDetailsRepository;
        private IUserRepository _userRepository;
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

        public DALContainer(string connectionString)
        {
            var dataBaseRepository = new DataBaseRepository(connectionString);
            var baseMapper = new BaseMapper();

            _sampleEntityDetailsRepository = new SampleEntityDetailsRepository(dataBaseRepository, baseMapper);
            _sampleEntityRepository = new SampleEntityRepository(dataBaseRepository, _sampleEntityDetailsRepository, baseMapper);
            _userRepository = new UserRepository(dataBaseRepository, baseMapper);
            _companyRepository = new CompanyRepository(dataBaseRepository, baseMapper);
        }
    }
}
