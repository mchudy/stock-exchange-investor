using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;

namespace StockExchange.Business.Indicators
{
    /// <summary>
    /// Volume Price Trend technical indicator
    /// </summary>
    public class VptIndicator : IIndicator
    {
        /// <inheritdoc />
        public IndicatorType Type => IndicatorType.Vpt;

        /// <inheritdoc />
        public IList<IndicatorValue> Calculate(IList<Price> prices)
        {
            var ret = new List<IndicatorValue> { new IndicatorValue { Date = prices[0].Date, Value = 0m } };
            var lastValue = 0m;
            for (var i = 1; i < prices.Count; ++i)
            {
                var val = lastValue + prices[i].Volume * (prices[i].ClosePrice - prices[i - 1].ClosePrice) / prices[i - 1].ClosePrice;
                lastValue = val;
                ret.Add(new IndicatorValue { Date = prices[i].Date, Value = val });
            }
            return ret;
        }

        /// <inheritdoc />
        public IList<Signal> GenerateSignals(IList<Price> prices)
        {
            var signals = new List<Signal>();
            var values = Calculate(prices);
            const int priceTerm = 14;
            var priceTrend = MovingAverageHelper.ExpotentialMovingAverage(prices, priceTerm);
            SignalAction lastAction = SignalAction.NoSignal;
            for (int i = priceTerm; i < prices.Count; i++)
            {
                if(values[i-1].Value < values[i].Value && priceTrend[i-priceTerm].Value < priceTrend[i - priceTerm + 1].Value)
                {
                    if (lastAction != SignalAction.Buy)
                    {
                        signals.Add(new Signal(SignalAction.Buy) {Date = values[i].Date});
                        lastAction = SignalAction.Buy;
                    } 
                }
                if (values[i - 1].Value <= values[i].Value ||
                    priceTrend[i - priceTerm].Value <= priceTrend[i - priceTerm + 1].Value) continue;
                if (lastAction == SignalAction.Sell) continue;
                signals.Add(new Signal(SignalAction.Sell) {Date = values[i].Date});
                lastAction = SignalAction.Sell;
            }
            return signals;
        }
    }
}
