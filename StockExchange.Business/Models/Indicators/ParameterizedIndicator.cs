using System.Collections.Generic;
using StockExchange.Business.Indicators;

namespace StockExchange.Business.Models.Indicators
{
    public class ParameterizedIndicator
    {
        public IndicatorType? IndicatorType { get; set; }

        public IList<IndicatorProperty> Properties { get; set; } 
    }
}
