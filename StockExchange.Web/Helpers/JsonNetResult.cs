using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StockExchange.Common;
using StockExchange.Common.Extensions;

namespace StockExchange.Web.Helpers
{
    public class JsonNetResult : ActionResult
    {
        //private Type type;
        //private string fieldName;

        private readonly string _dataFormatString;

        public Encoding ContentEncoding { get; set; }

        public string ContentType { get; set; }

        public object Data { get; set; }

        public JsonSerializerSettings SerializerSettings { get; set; }

        public Formatting Formatting { get; set; }

        public JsonNetResult(object data, Formatting formatting) : this(data)
        {
            Formatting = formatting;
        }

        public JsonNetResult(object data)
        {
            Data = data;
            Formatting = Formatting.None;
            SerializerSettings = new JsonSerializerSettings { ContractResolver = new CustomFormatResolver(_dataFormatString) };
        }

        public JsonNetResult()
        {
            Formatting = Formatting.None;
            SerializerSettings = new JsonSerializerSettings { ContractResolver = new CustomFormatResolver(_dataFormatString) };
        }

        public JsonNetResult(object data, Type type, string fieldName) : this(data)
        {
            var attribute = AttributeHelper.GetPropertyAttribute<DisplayFormatAttribute>(type, fieldName);
            _dataFormatString = attribute?.DataFormatString;
            Formatting = Formatting.None;
            SerializerSettings = new JsonSerializerSettings { ContractResolver = new CustomFormatResolver(_dataFormatString) };
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";
            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;
            if (Data == null) return;
            var writer = new JsonTextWriter(response.Output) { Formatting = Formatting };
            var serializer = JsonSerializer.Create(SerializerSettings);
            serializer.Serialize(writer, Data);
            writer.Flush();
        }
    }

    public class CustomFormatResolver : DefaultContractResolver
    {
        private readonly string _dataFormatString;

        public CustomFormatResolver(string dataFormatString)
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

    internal class DisplayFormatConverter : JsonConverter
    {
        private readonly string _dataFormat;

        public DisplayFormatConverter(string dataFormat)
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

    internal class EnumDisplayFormatConverter : JsonConverter
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