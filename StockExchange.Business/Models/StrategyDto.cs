using System.Collections.Generic;
using StockExchange.Business.Models.Indicators;

namespace StockExchange.Business.Models
{
    public class StrategyDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }

        public IDictionary<IndicatorProperty, string> Indicators { get; set; }
    }
}