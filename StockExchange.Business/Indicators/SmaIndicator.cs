using System.Collections.Generic;
using System.Linq;
using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;

namespace StockExchange.Business.Indicators
{
    public class SmaIndicator : IIndicator
    {
        public const int DefaultTerm = 5;

        public IndicatorType Type => IndicatorType.Sma;

        public int Term { get; set; } = DefaultTerm;

        public IList<IndicatorValue> Calculate(IList<Price> prices)
        {
            var ret = new List<IndicatorValue>();
            for (var i = 0; i < prices.Count - Term + 1; ++i)
                ret.Add(MovingAverageHelper.SimpleMovingAverage(prices.Skip(i).Take(Term).ToList()));
            return ret;
        }

        public IList<Signal> GenerateSignals(IList<Price> prices)
        {
            var signals = new List<Signal>();
            return signals;
        }
    }
}
