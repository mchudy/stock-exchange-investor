using System.Collections.Generic;
using StockExchange.Business.Models.Indicators;

namespace StockExchange.Web.Models
{
    public class StrategyViewModel
    {
        public IList<IndicatorViewModel> Indicators { get; set; }

        public IDictionary<IndicatorProperty, IndicatorViewModel> Properties { get; set; }
    }
}