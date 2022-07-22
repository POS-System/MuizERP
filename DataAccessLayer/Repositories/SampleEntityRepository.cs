using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities;
using Entities.Base;
using Entities.Base.Utils.ParametersContainers;

namespace DataAccessLayer.Repositories
{
    internal class SampleEntityRepository : ISampleEntityRepository
    {
        private readonly DataRepository _dataRepository;
        private readonly ISampleEntityDetailsRepository _sampleEntitiesDetailsRepository;
        private readonly IDataMapper _dataMapper;

        public SampleEntityRepository(
            DataRepository dataRepository,
            ISampleEntityDetailsRepository sampleEntitiesDetailsRepository,
            IDataMapper dataMapper)
        {
            _dataRepository = dataRepository;
            _sampleEntitiesDetailsRepository = sampleEntitiesDetailsRepository;
            
            _dataMapper = dataMapper;
        }

        public EntityCollection<SampleEntity> GetCollection(IParametersContainer parameters)
        {
            var result = new EntityCollection<SampleEntity>();

            _dataRepository.ReadCollectionWithSchema<SampleEntity>(
                cmd => cmd.AddParameters(parameters),
                drd =>
                {
                    var item = new SampleEntity();
                    _dataMapper.Map(drd, item);

                    // Собираем параметры  для удобной передачи в методы
                    var detailsParams = new ParametersContainer();
                    detailsParams.Add<SampleEntity>(nameof(item.ID), item.ID);

                    item.SampleEntityDetailsList = _sampleEntitiesDetailsRepository.GetCollection(detailsParams);
                    
                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(SampleEntity item)
        {
            _dataRepository.DoInTransaction(conn =>
            {
                _dataRepository.SaveBaseItem(item, conn, null);
                _dataRepository.SaveCollection(
                    item.SampleEntityDetailsList,
                    entityDetail =>
                    {
                        entityDetail.SampleEntityID = item.ID;
                        _sampleEntitiesDetailsRepository.SaveItem(entityDetail, conn);
                    });
            });            
        }  
    }
}
