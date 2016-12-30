using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Indicators;

namespace StockExchange.Business.Indicators
{
    public class ObvIndicator : IIndicator
    {
        public IndicatorType Type => IndicatorType.Obv;

        public IList<IndicatorValue> Calculate(IList<Price> prices)
        {
            var values = new List<IndicatorValue>
            {
                new IndicatorValue { Date = prices[0].Date, Value = prices[0].Volume }
            };
            var lastValue = prices[0].Volume;
            for (var i = 1; i < prices.Count; i++)
            {
                var value = lastValue;
                var todayClosePrice = prices[i].ClosePrice;
                var previousClosePrice = prices[i - 1].ClosePrice;
                if (todayClosePrice > previousClosePrice)
                {
                    value += prices[i].Volume;
                }
                else if (todayClosePrice < previousClosePrice)
                {
                    value -= prices[i].Volume;
                }
                lastValue = value;
                values.Add(new IndicatorValue { Date = prices[i].Date, Value = value });
            }
            return values;
        }

        public IList<Signal> GenerateSignals(IList<Price> prices)
        {
            var signals = new List<Signal>();
            var values = Calculate(prices);
            const int trendTerm = 20;
            var trend = MovingAverageHelper.ExpotentialMovingAverage(values, trendTerm);
            SignalAction lastAction = SignalAction.NoSignal;
            for (int i = trendTerm; i < prices.Count; i++)
            {
                if (trend[i - trendTerm].Value < trend[i - trendTerm + 1].Value && values[i].Value > prices[i].Volume)
                {
                    if (lastAction != SignalAction.Sell)
                    {
                        signals.Add(new Signal(SignalAction.Sell) {Date = prices[i].Date});
                        lastAction = SignalAction.Sell;
                    }
                }
                else if(trend[i-trendTerm].Value > trend[i-trendTerm+1].Value && values[i].Value < prices[i].Volume)
                {
                    if (lastAction != SignalAction.Buy)
                    {
                        signals.Add(new Signal(SignalAction.Buy) {Date = prices[i].Date});
                        lastAction = SignalAction.Buy;
                    }
                }
            } 
            return signals;
        }
    }
}
