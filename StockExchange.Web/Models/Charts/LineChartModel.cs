using System.Collections.Generic;

namespace StockExchange.Web.Models.Charts
{
    public class LineChartModel
    {
        public int CompanyId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// List of points in format [javaScriptTimestamp, value]
        /// </summary>
        public IList<decimal[]> Data { get; set; }
    }
}