using StockExchange.Business.Models.Indicators;
using StockExchange.Web.Models.Indicator;
using System.Collections.Generic;

namespace StockExchange.Web.Models.Strategy
{
    public class StrategyViewModel
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public IList<IndicatorViewModel> Indicators { get; set; }

        public IDictionary<IndicatorProperty, IndicatorViewModel> Properties { get; set; }
    }
}