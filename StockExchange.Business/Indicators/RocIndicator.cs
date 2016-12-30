using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using StockExchange.Business.Indicators.Common;

namespace StockExchange.Business.Indicators
{
    public class RocIndicator : IIndicator
    {
        public const int DefaultRocTerm = 12;

        public int Term { get; set; } = DefaultRocTerm;

        public IndicatorType Type => IndicatorType.Roc;

        public IList<IndicatorValue> Calculate(IList<Price> prices)
        {
            var values = new List<IndicatorValue>();
            for (var i = Term; i < prices.Count; i++)
            {
                var value = (prices[i].ClosePrice - prices[i - Term].ClosePrice) / prices[i - Term].ClosePrice * 100;
                values.Add(new IndicatorValue
                {
                    Date = prices[i].Date,
                    Value = value
                });
            }
            return values;
        }

        public IList<Signal> GenerateSignals(IList<Price> prices)
        {
            var signals = new List<Signal>();
            var values = Calculate(prices);
            var trend = MovingAverageHelper.ExpotentialMovingAverage(prices, Term);
            SignalAction lastAction = SignalAction.NoSignal;
            for (int i = Term; i < prices.Count; i++)
            {
                if (trend[i-1].Value < trend[i].Value && prices[i].ClosePrice > trend[i].Value && values[i].Value<0)
                {
                    if (lastAction != SignalAction.Buy)
                    {
                        signals.Add(new Signal(SignalAction.Buy) {Date = values[i].Date});
                        lastAction = SignalAction.Buy;
                    }
                }
                else if (trend[i - 1].Value > trend[i].Value && prices[i].ClosePrice < trend[i].Value && values[i].Value > 0)
                {
                    if (lastAction != SignalAction.Sell)
                    {
                        signals.Add(new Signal(SignalAction.Sell) {Date = values[i].Date});
                        lastAction = SignalAction.Sell;
                    }
                }
            }
            return signals;
        }
    }
}