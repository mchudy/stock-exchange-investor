using Newtonsoft.Json;
using System.Collections.Generic;

namespace StockExchange.Web.Models.DataTables
{
    public sealed class DataTableResponse<T>
    {
        [JsonProperty(PropertyName = "draw")]
        public int Draw { get; set; }

        [JsonProperty(PropertyName = "recordsTotal")]
        public int RecordsTotal { get; set; }

        [JsonProperty(PropertyName = "recordsFiltered")]
        public int RecordsFiltered { get; set; }

        [JsonProperty(PropertyName = "data")]
        public IEnumerable<T> Data { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }
}
