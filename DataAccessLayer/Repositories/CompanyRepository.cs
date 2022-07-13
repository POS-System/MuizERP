using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities;
using Entities.Base;
using Entities.Base.Parameters;

namespace DataAccessLayer.Repositories
{
    internal class CompanyRepository : ICompanyRepository
    {
        private readonly DataBaseRepository _dataBaseRepository;
        private readonly IMapper<SqlDataReaderWithSchema, BaseEntity> _baseMapper;

        public CompanyRepository(
            DataBaseRepository dataBaseRepository,
            IMapper<SqlDataReaderWithSchema, BaseEntity> baseMapper)
        {
            _dataBaseRepository = dataBaseRepository;
            _baseMapper = baseMapper;
        }

        public EntityCollection<Company> GetItems(IParametersContainer parameters)
        {
            var result = new EntityCollection<Company>();

            _dataBaseRepository.ReadCollectionWithSchema<Company>(
                cmd => SqlCommandConfigurator.Configure(cmd, parameters),
                drd =>
                {
                    var item = new Company();
                    _baseMapper.Map(drd, item);

                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(Company item)
        {
            _dataBaseRepository.DoInTransaction(conn => _dataBaseRepository.SaveBaseItem(item, conn));
        }
    }
}
