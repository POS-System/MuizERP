using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Parameters;
using Entities.Base;
using Entities.SampleEntity;
using Entities.SampleEntityDetailsN;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    internal class SampleEntityDAL : IEntityDAL<SampleEntity>
    {
        private DataBaseDAL _dataBaseDAL;
        private IEntityDAL<SampleEntityDetails> _sampleEntitiesDetailsDAL;
        private IMapper<SqlDataReaderWithSchema, object> _mapper;

        public SampleEntityDAL(DataBaseDAL dataBaseDAL, IMapper<SqlDataReaderWithSchema, object> mapper, IEntityDAL<SampleEntityDetails> sampleEntitiesDetailsDAL = null)
        {
            _dataBaseDAL = dataBaseDAL;
            _sampleEntitiesDetailsDAL = sampleEntitiesDetailsDAL;
            _mapper = mapper;
        }

        public delegate void AfterGetData(BaseEntity item);

        public ObservableCollection<SampleEntity> GetItemsLambda(IParametersContainer parametersContainer)
        {
            var result = GetItemsLambda<SampleEntity>(
                parametersContainer,
                item =>
                {
                    // Собираем параметры  для удобной передачи в методы
                    var parameters = new ParametersContainer();
                    parameters.Add<SampleEntity>(nameof(item.ID), item.ID);
                    ((SampleEntity)item).SampleEntityDetailsList = _sampleEntitiesDetailsDAL.GetItems(parameters);
                });
            return result;
        }

        public ObservableCollection<T> GetItemsLambda<T>(IParametersContainer parametersContainer, AfterGetData afterGetData) where T : BaseEntity, new()
        {
            var result = new ObservableCollection<T>();

            _dataBaseDAL.ReadCollectionWithSchema<T>(
                sqlCmd => ParametersConfigurator.ConfigureSqlCommand(sqlCmd, parametersContainer),
                drd =>
                {
                    var item = new T();
                    _mapper.Map(drd, item);
                    
                    afterGetData(item);

                    result.Add(item);
                });

            return result;
        }

        public ObservableCollection<SampleEntity> GetItems(IParametersContainer parametersContainer)
        {
            var result = new ObservableCollection<SampleEntity>();

            _dataBaseDAL.ReadCollectionWithSchema<SampleEntity>(
                sqlCmd =>
                {                    
                    ParametersConfigurator.ConfigureSqlCommand(sqlCmd, parametersContainer);
                },
                drd =>
                {
                    var item = new SampleEntity();
                    _mapper.Map(drd, item);

                    // Собираем параметры  для удобной передачи в методы
                    var parameters = new ParametersContainer();
                    parameters.Add<SampleEntity>(nameof(item.ID), item.ID);
                    item.SampleEntityDetailsList = _sampleEntitiesDetailsDAL.GetItems(parameters);
                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(SampleEntity baseEntity, SqlConnection sqlConnection)
        {
            _dataBaseDAL.DoInTransaction(conn =>
            {
                _dataBaseDAL.SetBaseItem(baseEntity, conn, null);
                _dataBaseDAL.SaveCollection(baseEntity.SampleEntityDetailsList,
                    sampleEntityDetail =>
                    {
                        sampleEntityDetail.SampleEntityID = baseEntity.ID;
                        _sampleEntitiesDetailsDAL.SaveItem(sampleEntityDetail, conn);
                    });
            });            
        }  
    }
}
