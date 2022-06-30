using DataAccessLayer.Mapping;
using Entities.Company;
using Entities.SampleEntity;
using Entities.SampleEntityDetailsN;
using Entities.User;

namespace DataAccessLayer
{
    public class ContainerDAL
    {
        private SampleEntityDAL _sampleEntityDAL;
        private SampleEntityDetailsDAL _sampleEntityDetailsDAL;
        private UserDAL _userDAL;
        private CompanyDAL _companyDAL;

        public IEntityDAL<SampleEntity> SampleEntityDAL
        {
            get { return _sampleEntityDAL; }
        }

        public IEntityDAL<SampleEntityDetails> SampleEntityDetailsDAL
        {
            get { return _sampleEntityDetailsDAL; }
        }

        public IEntityDAL<User> UserDAL
        {
            get { return _userDAL; }
        }

        public IEntityDAL<Company> CompanyDAL
        {
            get { return _companyDAL; }
        }

        public ContainerDAL(string connectionString)
        {
            DataBaseDAL dataBaseDAL = new DataBaseDAL(connectionString);
            BaseMapper baseMapper = new BaseMapper();

            _sampleEntityDetailsDAL = new SampleEntityDetailsDAL(dataBaseDAL, baseMapper);
            _sampleEntityDAL = new SampleEntityDAL(dataBaseDAL, baseMapper, _sampleEntityDetailsDAL);
            _userDAL = new UserDAL(dataBaseDAL, baseMapper);
            _companyDAL = new CompanyDAL(dataBaseDAL, baseMapper);
        }
    }
}
