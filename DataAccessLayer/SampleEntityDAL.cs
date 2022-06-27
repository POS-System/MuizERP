using DataAccessLayer.DataReaders;
using DataAccessLayer.Delegates;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Parameters;
using Entities.Base;
using Entities.SampleEntity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace DataAccessLayer
{
    internal class SampleEntityDAL : IEntityDAL<SampleEntity>
    {
        private DataBaseDAL dataBaseDAL;
        private IEntityDAL<SampleEntityDetails> sampleEntitiesDetailsDAL;
        private IConvert<SqlDataReaderWithSchema, object> convertor;

        public SampleEntityDAL(DataBaseDAL _dataBaseDAL, IConvert<SqlDataReaderWithSchema, object> _convertor, IEntityDAL<SampleEntityDetails> _sampleEntityDetailsDAL = null)
        {
            dataBaseDAL = _dataBaseDAL;
            sampleEntitiesDetailsDAL = _sampleEntityDetailsDAL;
            convertor = _convertor;
        }

        public ObservableCollection<SampleEntity> GetItems(IParametersContainer parametersContainer)
        {
            var result = new ObservableCollection<SampleEntity>();

            dataBaseDAL.ReadCollectionWithSchema(
                sqlCmd =>
                {
                    sqlCmd.CommandText = "xp_GetSampleEntities";
                },
                drd =>
                {
                    var item = new SampleEntity();
                    convertor.Convert(drd, item);

                    // Собираем параметры  для удобной передачи в методы
                    var parameters = new ParametersContainer();
                    parameters.Add<SampleEntity>(nameof(item.ID), item.ID);

                    item.SampleEntityDetailsList = sampleEntitiesDetailsDAL.GetItems(parameters);
                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(SampleEntity _baseEntity, SqlConnection _conn)
        {            
            dataBaseDAL.DoInTransaction(conn =>
            {
                dataBaseDAL.SetBaseItem(_baseEntity, conn, null);
                dataBaseDAL.SaveCollection(_baseEntity.SampleEntityDetailsList,
                    sampleEntityDetail =>
                    {
                        sampleEntityDetail.SampleEntityID = _baseEntity.ID;
                        sampleEntitiesDetailsDAL.SaveItem(sampleEntityDetail, conn);
                    });
            });            
        }  
    }
}
