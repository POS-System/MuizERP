using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Parameters;
using DataAccessLayer.Repositories.Interfaces;
using Entities.Base;
using Entities.Base.Parameters;
using Entities.SampleEntityDetailsN;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace DataAccessLayer.Repositories
{
    internal sealed class SampleEntityDetailsRepository : ISampleEntityDetailsRepository
    {        
        private readonly DataBaseRepository _dataBaseRepository;
        private readonly IMapper<SqlDataReaderWithSchema, BaseEntity> _baseMapper;

        public SampleEntityDetailsRepository(DataBaseRepository dataBaseRepository, IMapper<SqlDataReaderWithSchema, BaseEntity> baseMapper)
        {
            _dataBaseRepository = dataBaseRepository;
            _baseMapper = baseMapper;
        }

        public ObservableCollection<SampleEntityDetails> GetItems(IParametersContainer parametersContainer)
        {
            var result = new ObservableCollection<SampleEntityDetails>();

            _dataBaseRepository.ReadCollectionWithSchema<SampleEntityDetails>(
                sqlCmd => ParametersConfigurator.ConfigureSqlCommand(sqlCmd, parametersContainer),
                drd =>
                {
                    var item = new SampleEntityDetails();
                    _baseMapper.Map(drd, item);
                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(SampleEntityDetails sampleEntityDetails, SqlConnection conn)
        {
            _dataBaseRepository.SaveBaseItem(sampleEntityDetails, conn);
        }
    }
}
