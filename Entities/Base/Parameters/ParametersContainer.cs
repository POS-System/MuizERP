using System.Collections.Generic;

namespace Entities.Base.Parameters
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
            if (_parameters.ContainsKey(name))
                _parameters[name] = value;
            else
                _parameters.Add(name, value);
        }

        public void Add<T>(string fieldName, object value)
        {
            var name = $"{typeof(T).Name}{fieldName}";
            Add(name, value);

        }

        public void Remove(string name)
        {
            if (_parameters.ContainsKey(name))
                _parameters.Remove(name);
        }

        public void Clear()
        {
            _parameters.Clear();
        }

        public Dictionary<string, object> GetParameters()
        {
            return _parameters;
        }
    }
}
