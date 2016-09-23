using System;
using System.Reflection;

namespace StockExchange.Common
{
    public static class AttributeHelper
    {
        public static T GetPropertyAttribute<T>(Type type, string propertyName) where T : Attribute
        {
            var property = type.GetProperty(propertyName);
            return (T)property.GetCustomAttribute(typeof(T));
        }
    }
}
