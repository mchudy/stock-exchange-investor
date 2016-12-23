using StockExchange.Business.Models.Indicators;
using System.Collections.Generic;

namespace StockExchange.Business.Models.Strategy
{
    public class StrategyDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }

        public IList<ParameterizedIndicator> Indicators { get; set; }
    }
}
