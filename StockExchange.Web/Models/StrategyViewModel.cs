using System.Collections.Generic;

namespace StockExchange.Web.Models
{
    public class StrategyViewModel
    {
        public IList<IndicatorViewModel> Indicators { get; set; }
    }
}