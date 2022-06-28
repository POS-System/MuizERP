using Entities.Base;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DataAccessLayer.Parameters
{
    public class ParametersContainer : IParametersContainer
    {
        private Dictionary<string, object> _parameters;

        public ParametersContainer()
        {
            _parameters = new Dictionary<string, object>();
        }

        public void Add(string name, object value)
        {
            if (!_parameters.ContainsKey(name))
                _parameters.Add(name, value);
        }

        public void Add<T>(string fieldName, object value)
        {
            var name = $"{typeof(T).Name}{fieldName}";
            Add(name, value);

        }

        public void AddRange(IFilter filter)
        {
            _parameters.Clear();
            var type = filter.GetType();
            var props = type.GetProperties();
            foreach (var property in props)
            {  
                var parameterName = property.Name;
                var value = property.GetValue(filter, null);
                Add(parameterName, value);
            }            
        }

        public void Remove(string name)
        {
            if (_parameters.ContainsKey(name))
                _parameters.Remove(name);
        }

        public Dictionary<string, object> GetParameters()
        {
            return _parameters;
        }
    }
}
