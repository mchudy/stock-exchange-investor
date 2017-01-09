using System.Collections.Generic;

namespace StockExchange.Web.Models.Charts
{
    /// <summary>
    /// View model for line charts
    /// </summary>
    public class LineChartModel
    {
        /// <summary>
        /// Company Id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Company name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of points in format [javaScriptTimestamp:int, value]
        /// </summary>
        public IList<decimal[]> Data { get; set; }
    }
}