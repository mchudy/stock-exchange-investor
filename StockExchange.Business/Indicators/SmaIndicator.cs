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
            var values = Calculate(prices);
            var lastAction = SignalAction.NoSignal;
            for (int i = Term; i < prices.Count; i++)
            {
                if (values[i - Term].Value < values[i - Term + 1].Value && prices[i].ClosePrice > values[i - Term + 1].Value)
                {
                    if (lastAction == SignalAction.Buy) continue;
                    signals.Add(new Signal(SignalAction.Buy) { Date = prices[i].Date });
                    lastAction = SignalAction.Buy; ;
                }
                else if (values[i - Term].Value > values[i - Term + 1].Value && prices[i].ClosePrice < values[i - Term + 1].Value)
                {
                    if (lastAction == SignalAction.Sell) continue;
                    signals.Add(new Signal(SignalAction.Sell) { Date = prices[i].Date });
                    lastAction = SignalAction.Sell;
                }
            }
            return signals;
        }
    }
}
