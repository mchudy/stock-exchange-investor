using System.Collections.Generic;

namespace StockExchange.Web.Models.Charts
{
    public class PieChartModel
    {
        public string Title { get; set; }
        public IList<PieChartEntry> Data { get; set; } = new List<PieChartEntry>();
    }

    public class PieChartEntry
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
    }
}