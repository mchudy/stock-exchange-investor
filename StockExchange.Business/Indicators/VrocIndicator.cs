using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using StockExchange.Business.Indicators.Common;

namespace StockExchange.Business.Indicators
{
    public class VrocIndicator : IIndicator
    {
        public const int DefaultVrocTerm = 14;

        public int Term { get; set; } = DefaultVrocTerm;

        public IndicatorType Type => IndicatorType.Vroc;

        public IList<IndicatorValue> Calculate(IList<Price> prices)
        {
            var values = new List<IndicatorValue>();
            for (var i = Term; i < prices.Count; i++)
            {
                var value = ((decimal)(prices[i].Volume - prices[i - Term].Volume)) / prices[i - Term].Volume * 100;
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
            var priceTrend = MovingAverageHelper.ExpotentialMovingAverage(prices, Term);
            SignalAction lastAction = SignalAction.NoSignal;
            for (int i = Term; i < prices.Count; i++)
            {
                if (prices[i].ClosePrice > priceTrend[i - Term + 1].Value && priceTrend[i - Term].Value < priceTrend[i - Term + 1].Value && values[i - Term].Value < 0)
                {
                    if (lastAction != SignalAction.Buy)
                    {
                        signals.Add(new Signal(SignalAction.Buy) { Date = prices[i].Date });
                        lastAction = SignalAction.Buy;
                    }
                }
                else if (prices[i].ClosePrice < priceTrend[i - Term + 1].Value && priceTrend[i - Term].Value > priceTrend[i - Term + 1].Value && values[i - Term].Value > 0)
                {
                    if (lastAction != SignalAction.Sell)
                    {
                        signals.Add(new Signal(SignalAction.Sell) { Date = prices[i].Date });
                        lastAction = SignalAction.Sell;
                    }
                }
            }
            return signals;
        }
    }
}