using StockExchange.Business.Indicators.Common;
using System.Collections.Generic;

namespace StockExchange.Business.Models.Indicators
{
    public class ParameterizedIndicator
    {
        public IndicatorType? IndicatorType { get; set; }

        public IList<IndicatorProperty> Properties { get; set; } 
    }
}
