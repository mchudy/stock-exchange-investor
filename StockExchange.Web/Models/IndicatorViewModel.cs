using StockExchange.Business.Indicators;

namespace StockExchange.Web.Models
{
    public class IndicatorViewModel
    {
        public IndicatorType Type { get; set; }
        public string Name { get; set; }
    }
}