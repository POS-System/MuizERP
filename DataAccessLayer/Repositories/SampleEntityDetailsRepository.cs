using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities;
using Entities.Base;
using Entities.Base.Utils.Interface;
using System.Data.SqlClient;

namespace DataAccessLayer.Repositories
{
    internal sealed class SampleEntityDetailsRepository : ISampleEntityDetailsRepository
    {        
        private readonly DataRepository _dataRepository;
        private readonly IDataMapper _dataMapper;

        public SampleEntityDetailsRepository(
            DataRepository dataRepository,
            IDataMapper dataMapper)
        {
            _dataRepository = dataRepository;

            _dataMapper = dataMapper;
        }

        public EntityCollection<SampleEntityDetails> GetItems(IParametersContainer parameters)
        {
            var result = new EntityCollection<SampleEntityDetails>();

            _dataRepository.ReadCollectionWithSchema<SampleEntityDetails>(
                cmd => cmd.ConfigureParameters(parameters),
                drd =>
                {
                    var item = new SampleEntityDetails();
                    _dataMapper.Map(drd, item);

                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(SampleEntityDetails item, SqlConnection conn)
        {
            _dataRepository.SaveBaseItem(item, conn);
        }
    }
}
