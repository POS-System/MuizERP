using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities;
using Entities.Base;
using Entities.Base.Utils;
using Entities.Base.Utils.Interface;

namespace DataAccessLayer.Repositories
{
    internal class SampleEntityRepository : ISampleEntityRepository
    {
        private readonly DataBaseRepository _dataBaseRepository;
        private readonly ISampleEntityDetailsRepository _sampleEntitiesDetailsRepository;
        private readonly IMapper<SqlDataReaderWithSchema, BaseEntity> _baseMapper;

        public SampleEntityRepository(
            DataBaseRepository dataBaseRepository,
            ISampleEntityDetailsRepository sampleEntitiesDetailsRepository,
            IMapper<SqlDataReaderWithSchema, BaseEntity> baseMapper)
        {
            _dataBaseRepository = dataBaseRepository;
            _sampleEntitiesDetailsRepository = sampleEntitiesDetailsRepository;
            _baseMapper = baseMapper;
        }

        public EntityCollection<SampleEntity> GetItems(IParametersContainer parameters)
        {
            var result = new EntityCollection<SampleEntity>();

            _dataBaseRepository.ReadCollectionWithSchema<SampleEntity>(
                cmd => cmd.ConfigureParameters(parameters),
                drd =>
                {
                    var item = new SampleEntity();
                    _baseMapper.Map(drd, item);

                    // Собираем параметры  для удобной передачи в методы
                    var detailsParams = new ParametersContainer();
                    detailsParams.Add<SampleEntity>(nameof(item.ID), item.ID);

                    item.SampleEntityDetailsList = _sampleEntitiesDetailsRepository.GetItems(detailsParams);
                    
                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(SampleEntity item)
        {
            _dataBaseRepository.DoInTransaction(conn =>
            {
                _dataBaseRepository.SaveBaseItem(item, conn, null);
                _dataBaseRepository.SaveCollection(
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
