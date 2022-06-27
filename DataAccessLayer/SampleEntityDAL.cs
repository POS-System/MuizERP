using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Parameters;
using Entities.SampleEntity;
using System.Collections.Generic;

namespace DataAccessLayer
{
    internal class SampleEntityDAL : ISampleEntityDAL
    {
        private IMapper<SqlDataReaderWithSchema, object> _baseMapper;
        
        private DataBaseDAL _dataBaseDAL;
        private ISampleEntityDetailsDAL _sampleEntitiesDetailsDAL;

        public SampleEntityDAL(
            DataBaseDAL dataBaseDAL,
            ISampleEntityDetailsDAL sampleEntityDetailsDAL,
            IMapper<SqlDataReaderWithSchema, object> baseMapper)
        {
            _dataBaseDAL = dataBaseDAL;
            _sampleEntitiesDetailsDAL = sampleEntityDetailsDAL;
            _baseMapper = baseMapper;
        }

        public IEnumerable<SampleEntity> GetSampleEntities()
        {
            var result = new List<SampleEntity>();

            _dataBaseDAL.ReadCollectionWithSchema(
                sqlCmd =>
                {
                    sqlCmd.CommandText = "xp_GetSampleEntities";
                },
                drd =>
                {
                    var item = new SampleEntity();
                    _baseMapper.Map(drd, item);

                    // Собираем параметры  для удобной передачи в методы
                    var parameters = new ParametersContainer();
                    parameters.Add<SampleEntity>(nameof(item.ID), item.ID);

                    item.SampleEntityDetailsList = _sampleEntitiesDetailsDAL.GetSampleEntityDetails(parameters);
                    result.Add(item);
                });

            return result;
        }

        public void SetSampleEntity(SampleEntity sampleEntity)
        {
            _dataBaseDAL.DoInTransaction(conn =>
            {
                // Получаем ID в демонстрационных целях
                var sampleEntityID = _dataBaseDAL.SetBaseItem(sampleEntity, conn, null);
                // Предлагаю вынести ID, CreatedDate, LastModifiedDate, CreatedByUserID, LastModifiedByUserID
                // в базовый объект

                _dataBaseDAL.SaveCollection(sampleEntity.SampleEntityDetailsList,
                    sampleEntityDetail =>
                    {
                        sampleEntityDetail.SampleEntityID = sampleEntityID;
                        _sampleEntitiesDetailsDAL.SetSampleEntityDetails(sampleEntityDetail, conn);
                    });
            });
        }
    }
}
