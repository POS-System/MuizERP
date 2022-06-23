using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Entities.Utils
{
    public static class TypeExtendingMethods
    {

        public static T GetCustomAttribute<T>(this Type type, bool inherit = true)
        {
            return type.GetCustomAttributes(typeof(T), inherit).Cast<T>().FirstOrDefault();
        }

        public static T GetCustomAttribute<T>(this PropertyInfo property, bool inherit = true) where T : Attribute
        {
            return property.GetCustomAttributes(typeof(T), inherit).Cast<T>().FirstOrDefault();
        }

        public static PropertyInfo[] GetCustomProperties<T>(this Type type)
        {
            return type.GetProperties().Where(p => typeof(T).IsAssignableFrom(p.PropertyType)).ToArray();
        }

        public static PropertyInfo[] GetCustomPropertiesWithAttribute<T, A>(this Type type) where A : Attribute
        {
            var result = type.GetCustomProperties<T>().Where(p => p.GetCustomAttribute<A>() != null).ToArray();
            return result;
        }

        public static PropertyInfo[] GetCustomPropertiesWithoutAttribute<T, A>(this Type type) where A : Attribute
        {
            var result = type.GetCustomProperties<T>().Where(p => p.GetCustomAttribute<A>() == null).ToArray();
            return result;
        }

        public static PropertyInfo[] GetCustomPropertiesWithAttribute<A>(this Type type) where A : Attribute
        {
            var result = type.GetProperties().Where(p => p.GetCustomAttribute<A>() != null).ToArray();
            return result;
        }

        public static PropertyInfo[] GetCustomPropertiesWithoutAttribute<A>(this Type type) where A : Attribute
        {
            var result = type.GetProperties().Where(p => p.GetCustomAttribute<A>() == null).ToArray();
            return result;
        }

        public static IEnumerable<FieldInfo> GetAllFields(this Type t)
        {
            if (t == null) return Enumerable.Empty<FieldInfo>();

            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                                       BindingFlags.Static | BindingFlags.Instance |
                                       BindingFlags.DeclaredOnly;

            return t.GetFields(flags).Concat(GetAllFields(t.BaseType));
        }
    }
}
