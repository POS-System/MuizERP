using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Parameters;
using Entities.Base;
using Entities.SampleEntityDetailsN;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    internal sealed class SampleEntityDetailsDAL : IEntityDAL<SampleEntityDetails>
    {        
        private DataBaseDAL _dataBaseDAL;
        private IMapper<SqlDataReaderWithSchema, BaseEntity> _mapper;

        public SampleEntityDetailsDAL(DataBaseDAL dataBaseDAL, IMapper<SqlDataReaderWithSchema, BaseEntity> mapper)
        {
            _dataBaseDAL = dataBaseDAL;
            _mapper = mapper;
        }

        public ObservableCollection<SampleEntityDetails> GetItems(IParametersContainer parametersContainer)
        {
            var result = new ObservableCollection<SampleEntityDetails>();

            _dataBaseDAL.ReadCollectionWithSchema<SampleEntityDetails>(
                sqlCommand =>
                {
                    ParametersConfigurator.ConfigureSqlCommand(sqlCommand, parametersContainer);
                },
                drd =>
                {
                    var item = new SampleEntityDetails();
                    _mapper.Map(drd, item);
                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(SampleEntityDetails sampleEntityDetails, SqlConnection conn)
        {
            _dataBaseDAL.SaveBaseItem(sampleEntityDetails, conn, null);
        }
    }
}
