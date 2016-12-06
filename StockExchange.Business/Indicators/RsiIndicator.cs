using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using StockExchange.Business.Models;

namespace StockExchange.Business.Indicators
{
    public class RsiIndicator : IIndicator
    {
        public const int DefaultRsiTerm = 14;

        public int Term { get; set; } = DefaultRsiTerm;

        public IList<IndicatorValue> Calculate(IList<Price> prices)
        {
            var gains = new List<IndicatorValue>();
            var losses = new List<IndicatorValue>();
            for (var i = 1; i < prices.Count; i++)
            {
                var change = prices[i].ClosePrice - prices[i - 1].ClosePrice;
                if (change > 0)
                {
                    gains.Add(new IndicatorValue { Date = prices[i].Date, Value = change });
                    losses.Add(new IndicatorValue { Date = prices[i].Date, Value = 0 });
                }
                else
                {
                    losses.Add(new IndicatorValue { Date = prices[i].Date, Value = -change });
                    gains.Add(new IndicatorValue { Date = prices[i].Date, Value = 0 });
                }
            }

            var averageGains = MovingAverageHelper.SmoothedMovingAverage(gains, Term);
            var averageLosses = MovingAverageHelper.SmoothedMovingAverage(losses, Term);

            return ComputeRsiValues(averageGains, averageLosses);
        }

        private static IList<IndicatorValue> ComputeRsiValues(IList<IndicatorValue> averageGains, IList<IndicatorValue> averageLosses)
        {
            var result = new List<IndicatorValue>();
            for (var i = 0; i < averageGains.Count; i++)
            {
                decimal rsi;
                if (averageLosses[i].Value != 0)
                {
                    var rs = averageGains[i].Value / averageLosses[i].Value;
                    rsi = 100m - 100m / (1m + rs);
                }
                else
                {
                    rsi = 100;
                }
                result.Add(new IndicatorValue { Date = averageGains[i].Date, Value = rsi });
            }
            return result;
        }
    }
}
