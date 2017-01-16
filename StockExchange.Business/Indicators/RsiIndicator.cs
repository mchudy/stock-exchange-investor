using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;

namespace StockExchange.Business.Indicators
{
    /// <summary>
    /// Relative Strength Index technical indicator
    /// </summary>
    [IndicatorDescription("Rsi")]
    public class RsiIndicator : IIndicator
    {
        /// <summary>
        /// Default <see cref="Term"/> value for the RSI indicator
        /// </summary>
        public const int DefaultRsiTerm = 14;

        /// <summary>
        /// Default <see cref="Minimum"/> value for the RSI indicator
        /// </summary>
        public const int DefaultMinimum = 30;

        /// <summary>
        /// Default <see cref="Maximum"/> value for the RSI indicator
        /// </summary>
        public const int DefaultMaximum = 70;

        /// <summary>
        /// The number of prices from previous days to include when computing 
        /// an indicator value
        /// </summary>
        public int Term { get; set; } = DefaultRsiTerm;

        /// <summary>
        /// The value of the lower RSI line (when crossed generates a signal)
        /// </summary>
        public int Minimum { get; set; } = DefaultMinimum;

        /// <summary>
        /// The value of the upper RSI line (when crossed generates a signal)
        /// </summary>
        public int Maximum { get; set; } = DefaultMaximum;

        /// <inheritdoc />
        [IgnoreIndicatorProperty]
        public IndicatorType Type => IndicatorType.Rsi;

        /// <inheritdoc />
        [IgnoreIndicatorProperty]
        public int RequiredPricesForSignalCount => Term + 1;

        /// <inheritdoc />
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

        /// <inheritdoc />
        public IList<Signal> GenerateSignals(IList<Price> prices)
        {
            var values = Calculate(prices);
            var signals = new List<Signal>();
            foreach (var indicatorValue in values)
            {
                if (indicatorValue.Value > Maximum)
                    signals.Add(new Signal(SignalAction.Sell) { Date = indicatorValue.Date });
                else if (indicatorValue.Value < Minimum)
                    signals.Add(new Signal(SignalAction.Buy) {Date = indicatorValue.Date});
            }
            return signals;
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
