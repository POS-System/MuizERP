using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Parameters;
using Entities.SampleEntity;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    internal sealed class SampleEntityDetailsDAL : ISampleEntityDetailsDAL
    {
        private IMapper<SqlDataReaderWithSchema, object> _baseMapper;
        private DataBaseDAL _dataBaseDAL;

        public SampleEntityDetailsDAL(
            DataBaseDAL dataBaseDAL,
            IMapper<SqlDataReaderWithSchema, object> baseMapper)
        {
            _dataBaseDAL = dataBaseDAL;
            _baseMapper = baseMapper;
        }

        public IEnumerable<SampleEntityDetails> GetSampleEntityDetails(ParametersContainer parameters)
        {
            var result = new List<SampleEntityDetails>();

            _dataBaseDAL.ReadCollectionWithSchema(
                sqlCmd =>
                {
                    sqlCmd.CommandText = "xp_GetSampleEntityDetails";
                    ParametersConfigurator.ConfigureSqlCommand(sqlCmd, parameters);
                    //sqlCmd.Parameters.AddWithValue("@p_sampleEntityID", sampleEntityID);
                },
                drd =>
                {
                    var item = new SampleEntityDetails();
                    _baseMapper.Map(drd, item);
                    result.Add(item);
                });

            return result;
        }

        public void SetSampleEntityDetails(SampleEntityDetails sampleEntityDetails, SqlConnection conn)
        {
            _dataBaseDAL.SetBaseItem(sampleEntityDetails, conn, null);
        }
    }
}
