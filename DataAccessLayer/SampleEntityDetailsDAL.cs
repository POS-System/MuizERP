using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Parameters;
using Entities.SampleEntityDetailsN;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    internal sealed class SampleEntityDetailsDAL : IEntityDAL<SampleEntityDetails>
    {        
        private DataBaseDAL _dataBaseDAL;
        private IMapper<SqlDataReaderWithSchema, object> _mapper;

        public SampleEntityDetailsDAL(DataBaseDAL dataBaseDAL, IMapper<SqlDataReaderWithSchema, object> mapper)
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
                read =>
                {
                    var item = new SampleEntityDetails();
                    _mapper.Map(read, item);
                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(SampleEntityDetails sampleEntityDetails, SqlConnection conn)
        {
            _dataBaseDAL.SetBaseItem(sampleEntityDetails, conn, null);
        }
    }
}
