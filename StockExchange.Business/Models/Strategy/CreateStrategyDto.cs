using System.Collections.Generic;
using StockExchange.Business.Models.Indicators;

namespace StockExchange.Business.Models.Strategy
{
    public class CreateStrategyDto
    {
        public string Name { get; set; }

        public int UserId { get; set; }

        public IList<ParameterizedIndicator> Indicators { get; set; }
    }
}