using DataAccessLayer.Parameters;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public interface IEntityDAL<T>
    {
        void SaveItem(T entity, SqlConnection conn = null); 

        ObservableCollection<T> GetItems(IParametersContainer parametersContainer);        
    }
}
