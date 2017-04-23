using System;
using Newtonsoft.Json;
using StockExchange.Common.Extensions;

namespace StockExchange.Web.Helpers.Json
{
    internal sealed class EnumDisplayFormatConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var @enum = value as Enum;
            if (@enum != null)
            {
                writer.WriteValue(@enum.GetEnumDescription());
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override bool CanRead => false;
    }
}