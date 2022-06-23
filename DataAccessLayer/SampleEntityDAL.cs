using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
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

                    item.SampleEntityDetailsList = _sampleEntitiesDetailsDAL.GetSampleEntityDetails(item.ID);
                    result.Add(item);
                });

            return result;
        }

        public void SetSampleEntity(SampleEntity sampleEntity)
        {
            _dataBaseDAL.DoInTransaction(conn =>
            {
                _dataBaseDAL.SetBaseItem(sampleEntity, conn, null);

                _dataBaseDAL.SaveCollection(sampleEntity.SampleEntityDetailsList,
                    sampleEntityDetail => _sampleEntitiesDetailsDAL.SetSampleEntityDetails(sampleEntityDetail, conn));
            });
        }
    }
}
