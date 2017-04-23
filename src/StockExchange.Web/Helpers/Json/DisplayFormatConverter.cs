using System;
using Newtonsoft.Json;

namespace StockExchange.Web.Helpers.Json
{
    internal sealed class DisplayFormatConverter : JsonConverter
    {
        private readonly string _dataFormat;

        internal DisplayFormatConverter(string dataFormat)
        {
            _dataFormat = dataFormat;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(string.Format(_dataFormat, value));
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