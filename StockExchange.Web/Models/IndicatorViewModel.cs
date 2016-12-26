using StockExchange.Business.Indicators;
using StockExchange.Business.Indicators.Common;

namespace StockExchange.Web.Models
{
    public class IndicatorViewModel
    {
        public IndicatorType Type { get; set; }
        public string Name { get; set; }
    }
}