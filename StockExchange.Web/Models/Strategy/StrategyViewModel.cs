using System.Collections.Generic;
using StockExchange.Business.Models.Indicators;
using StockExchange.Web.Models.Indicator;

namespace StockExchange.Web.Models.Strategy
{
    public class StrategyViewModel
    {
        public IList<IndicatorViewModel> Indicators { get; set; }

        public IDictionary<IndicatorProperty, IndicatorViewModel> Properties { get; set; }
    }
}