using DataAccessLayer.Parameters;
using Entities.Base;
using Entities.SampleEntityN;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public interface IEntityDAL<T>
    {
        void SaveItem(T sampleEntity, SqlConnection conn = null); 

        ObservableCollection<T> GetItems(IParametersContainer parametersContainer = null);        
    }
}
