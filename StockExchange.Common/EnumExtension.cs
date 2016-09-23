using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StockExchange.Common
{
    public static class EnumExtension
    {
        public static string GetEnumDescription<TEnum>(this TEnum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            if (fi == null) return null;
            var attributes = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);
            return attributes.Length > 0 ? attributes[0].Name : value.ToString();
        }

        public static TEnum GetEnumValueByDescription<TEnum>(string description)
        {
            var type = typeof(TEnum);
            type = Nullable.GetUnderlyingType(type) ?? type;
            if (string.IsNullOrEmpty(description) && Nullable.GetUnderlyingType(typeof(TEnum)) != null) return default(TEnum);
            if (!type.IsEnum)
                throw new InvalidOperationException($"{description} is not a valid description for enum {type.FullName}");
            var values = Enum.GetValues(type).Cast<TEnum>();
            foreach (var value in values.Where(value => string.Equals(value.GetEnumDescription(), description, StringComparison.InvariantCultureIgnoreCase)))
            {
                return value;
            }
            throw new InvalidOperationException($"{description} is not a valid description for enum {type.FullName}");
        }
    }
}
