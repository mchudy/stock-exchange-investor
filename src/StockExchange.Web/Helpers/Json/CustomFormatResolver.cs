using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace StockExchange.Web.Helpers.Json
{
    internal sealed class CustomFormatResolver : DefaultContractResolver
    {
        private readonly string _dataFormatString;

        internal CustomFormatResolver(string dataFormatString)
        {
            _dataFormatString = dataFormatString;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var attr = (DisplayFormatAttribute)member.GetCustomAttributes(typeof(DisplayFormatAttribute), true).SingleOrDefault();
            if (attr == null) return property;
            var converter = new DisplayFormatConverter(attr.DataFormatString);
            property.Converter = converter;
            return property;
        }

        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            var type = Nullable.GetUnderlyingType(objectType) ?? objectType;
            if (_dataFormatString != null && type.GetInterfaces().Contains(typeof(IFormattable)))
            {
                return new DisplayFormatConverter(_dataFormatString);
            }
            return objectType.IsEnum ? new EnumDisplayFormatConverter() : base.ResolveContractConverter(objectType);
        }
    }
}