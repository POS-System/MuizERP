using Entities.Base.Attributes;
using Entities.Base.Utils.Providers;
using Entities.Base.Utils.Validators;
using System.Reflection;

namespace Entities.Base.Providers
{
    public class NameByPropertyProvider : IKeyedProvider<PropertyInfo, string>
    {
        public string GetByValue(PropertyInfo property)
        {
            ArgumentValidator.ValidateThatArgumentNotNull(property, nameof(property));

            var attribute = property.GetCustomAttribute<SaveParameterAttribute>();

            if (attribute == null)
                return property.Name;
            else
            {
                return string.IsNullOrEmpty(attribute.Name)
                    ? property.Name
                    : attribute.Name;
            }
        }
    }
}
