using Newtonsoft.Json;
using System.Collections.Generic;

namespace StockExchange.Web.Models.DataTables
{
    /// <summary>
    /// Response model send to a DataTable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class DataTableResponse<T>
    {
        /// <summary>
        /// Draw
        /// </summary>
        [JsonProperty(PropertyName = "draw")]
        public int Draw { get; set; }

        /// <summary>
        /// Total number of items
        /// </summary>
        [JsonProperty(PropertyName = "recordsTotal")]
        public int RecordsTotal { get; set; }

        /// <summary>
        /// Number of records after filtering
        /// </summary>
        [JsonProperty(PropertyName = "recordsFiltered")]
        public int RecordsFiltered { get; set; }

        /// <summary>
        /// Filtered data
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// Error if any occurred
        /// </summary>
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }
}
