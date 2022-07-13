using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities;
using Entities.Base;
using Entities.Base.Parameters;

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

        public EntityCollection<SampleEntity> GetItems(IParametersContainer parametersContainer)
        {
            var result = new EntityCollection<SampleEntity>();

            _dataBaseRepository.ReadCollectionWithSchema<SampleEntity>(
                sqlCmd => ParametersConfigurator.ConfigureSqlCommand(sqlCmd, parametersContainer),
                drd =>
                {
                    var item = new SampleEntity();
                    _baseMapper.Map(drd, item);

                    // Собираем параметры  для удобной передачи в методы
                    var detailsParameters = new ParametersContainer();
                    detailsParameters.Add<SampleEntity>(nameof(item.ID), item.ID);
                    item.SampleEntityDetailsList = _sampleEntitiesDetailsRepository.GetItems(detailsParameters);
                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(SampleEntity sampleEntity)
        {
            _dataBaseRepository.DoInTransaction(conn =>
            {
                _dataBaseRepository.SaveBaseItem(sampleEntity, conn, null);
                _dataBaseRepository.SaveCollection(sampleEntity.SampleEntityDetailsList,
                    sampleEntityDetail =>
                    {
                        sampleEntityDetail.SampleEntityID = sampleEntity.ID;
                        _sampleEntitiesDetailsRepository.SaveItem(sampleEntityDetail, conn);
                    });
            });            
        }  
    }
}
