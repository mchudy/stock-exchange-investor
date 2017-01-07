using System.Collections.Generic;

namespace StockExchange.Web.Models.Charts
{
    /// <summary>
    /// View model for pie charts
    /// </summary>
    public class PieChartModel
    {
        /// <summary>
        /// Chart title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Chart data
        /// </summary>
        public IList<PieChartEntry> Data { get; set; } = new List<PieChartEntry>();
    }

    /// <summary>
    /// Single element in a pie chart
    /// </summary>
    public class PieChartEntry
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public decimal Value { get; set; }
    }
}