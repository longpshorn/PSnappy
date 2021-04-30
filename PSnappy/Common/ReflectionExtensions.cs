using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PSnappy
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<PropertyInfo> GetDeclaredUserProperties(this Type type)
        {
            return type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(pi => pi.PropertyType.IsValueType || pi.PropertyType == typeof(string) || pi.PropertyType == typeof(DateTime))
                .ToArray();
        }

        public static IEnumerable<PropertyInfo> GetDeclaredReadOnlyUserProperties(this Type type)
        {
            return GetDeclaredUserProperties(type).Where(pi => !pi.CanWrite);
        }

        public static IEnumerable<PropertyInfo> GetDeclaredReadWriteUserProperties(this Type type)
        {
            return GetDeclaredUserProperties(type).Where(pi => pi.CanWrite && pi.CanRead);
        }
    }
}
