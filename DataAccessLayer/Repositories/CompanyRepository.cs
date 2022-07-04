using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Parameters;
using DataAccessLayer.Repositories.Interfaces;
using Entities.Base;
using Entities.Base.Parameters;
using Entities.Company;
using System.Collections.ObjectModel;

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

        public ObservableCollection<Company> GetItems(IParametersContainer parametersContainer)
        {
            var result = new ObservableCollection<Company>();

            _dataBaseRepository.ReadCollectionWithSchema<Company>(
                sqlCmd => ParametersConfigurator.ConfigureSqlCommand(sqlCmd, parametersContainer),
                drd =>
                {
                    var item = new Company();
                    _baseMapper.Map(drd, item);
                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(Company company)
        {
            _dataBaseRepository.DoInTransaction(conn => _dataBaseRepository.SaveBaseItem(company, conn));
        }
    }
}
