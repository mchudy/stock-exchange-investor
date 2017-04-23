using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StockExchange.Common;

namespace StockExchange.Web.Helpers.Json
{
    internal sealed class JsonNetResult : ActionResult
    {
        private readonly string _dataFormatString;

        public Encoding ContentEncoding { get; set; }

        public string ContentType { get; set; }

        public object Data { get; set; }

        public JsonSerializerSettings SerializerSettings { get; set; }

        public Formatting Formatting { get; set; }

        public JsonNetResult(object data, bool camelCase = true)
        {
            Data = data;
            Formatting = Formatting.None;
            SerializerSettings = new JsonSerializerSettings();
            if (camelCase)
            {
                SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
            else
            {
                SerializerSettings.ContractResolver = new CustomFormatResolver(_dataFormatString);
            }
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
}