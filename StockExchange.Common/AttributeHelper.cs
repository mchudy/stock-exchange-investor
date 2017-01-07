using System;
using System.Reflection;

namespace StockExchange.Common
{
    /// <summary>
    /// Helper methods for dealing with attributes
    /// </summary>
    public static class AttributeHelper
    {
        /// <summary>
        /// Returns attribute on the property
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="type">Type</param>
        /// <param name="propertyName">The property name</param>
        /// <returns>The attribute</returns>
        public static T GetPropertyAttribute<T>(Type type, string propertyName) where T : Attribute
        {
            var property = type.GetProperty(propertyName);
            return (T)property.GetCustomAttribute(typeof(T));
        }
    }
}
