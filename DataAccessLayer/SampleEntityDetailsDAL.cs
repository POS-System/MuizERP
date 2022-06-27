using DataAccessLayer.DataReaders;
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
    internal sealed class SampleEntityDetailsDAL : IEntityDAL<SampleEntityDetails>
    {        
        private DataBaseDAL dataBaseDAL;
        private IConvert<SqlDataReaderWithSchema, object> convertor;

        public SampleEntityDetailsDAL(DataBaseDAL _dataBaseDAL, IConvert<SqlDataReaderWithSchema, object> _convertor)
        {
            dataBaseDAL = _dataBaseDAL;
            convertor = _convertor;
        }

        public ObservableCollection<SampleEntityDetails> GetItems(IParametersContainer parameters)
        {
            var result = new ObservableCollection<SampleEntityDetails>();

            dataBaseDAL.ReadCollectionWithSchema(
                sqlCmd =>
                {
                    sqlCmd.CommandText = "xp_GetSampleEntityDetails";
                    ParametersConfigurator.ConfigureSqlCommand(sqlCmd, parameters);                    
                },
                drd =>
                {
                    var item = new SampleEntityDetails();
                    convertor.Convert(drd, item);
                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(SampleEntityDetails _sampleEntityDetails, SqlConnection _conn)
        {
            dataBaseDAL.SetBaseItem(_sampleEntityDetails, _conn, null);
        }
    }
}
