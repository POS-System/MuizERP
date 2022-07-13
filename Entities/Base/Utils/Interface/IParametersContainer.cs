using System.Collections.Generic;

namespace Entities.Base.Utils.Interface
{
    public interface IParametersContainer
    {
        void Add(string name, object value);

        void Add<T>(string fieldName, object value);

        void Remove(string name);

        void Clear();        

        Dictionary<string, object> GetParameters();        
    }
}
