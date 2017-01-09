using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;

namespace StockExchange.Business.Indicators
{
    /// <summary>
    /// Rate of Change technical indicator
    /// </summary>
    public class RocIndicator : IIndicator
    {
        /// <summary>
        /// Default <see cref="Term"/> value for the ROC indicator
        /// </summary>
        public const int DefaultRocTerm = 12;

        /// <summary>
        /// The number of prices from previous days to include when computing 
        /// an indicator value
        /// </summary>
        public int Term { get; set; } = DefaultRocTerm;

        /// <inheritdoc />
        public IndicatorType Type => IndicatorType.Roc;

        /// <inheritdoc />
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

        /// <inheritdoc />
        public IList<Signal> GenerateSignals(IList<Price> prices)
        {
            var signals = new List<Signal>();
            var values = Calculate(prices);
            var trend = MovingAverageHelper.ExpotentialMovingAverage(prices, Term);
            SignalAction lastAction = SignalAction.NoSignal;
            for (int i = Term; i < prices.Count; i++)
            {
                if (prices[i].ClosePrice > trend[i-Term+1].Value && trend[i-Term].Value < trend[i-Term+1].Value && values[i-Term].Value<0)
                {
                    if (lastAction == SignalAction.Buy) continue;
                    signals.Add(new Signal(SignalAction.Buy) {Date = prices[i].Date});
                    lastAction = SignalAction.Buy;
                }
                else if (prices[i].ClosePrice < trend[i-Term+1].Value && trend[i-Term].Value > trend[i-Term+1].Value && values[i-Term].Value > 0)
                {
                    if (lastAction == SignalAction.Sell) continue;
                    signals.Add(new Signal(SignalAction.Sell) {Date = prices[i].Date});
                    lastAction = SignalAction.Sell;
                }
            }
            return signals;
        }
    }
}