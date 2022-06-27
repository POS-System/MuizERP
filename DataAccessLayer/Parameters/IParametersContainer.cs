using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Parameters
{
    public interface IParametersContainer
    {
        void Add(string name, object value);

        void Add<T>(string fieldName, object value);

        void Remove(string name);

        Dictionary<string, object> GetParameters();        
    }
}
