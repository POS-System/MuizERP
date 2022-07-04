using System.Collections.Generic;

namespace Entities.Base.Parameters
{
    public interface IParametersContainer
    {
        void Add(string name, object value);

        void Add<T>(string fieldName, object value);

        void Remove(string name);

        void Clear();

        void Clear();

        Dictionary<string, object> GetParameters();        
    }
}
