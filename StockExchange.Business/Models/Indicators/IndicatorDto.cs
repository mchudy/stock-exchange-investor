using StockExchange.Business.Indicators.Common;

namespace StockExchange.Business.Models.Indicators
{
    public class IndicatorDto
    {
        public IndicatorType IndicatorType { get; set; }

        public string IndicatorName { get; set; }
    }
}
