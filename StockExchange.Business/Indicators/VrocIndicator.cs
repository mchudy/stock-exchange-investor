using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;

namespace StockExchange.Business.Indicators
{
    /// <summary>
    /// Volume Rate of Change technical indicator
    /// </summary>
    public class VrocIndicator : IIndicator
    {
        /// <summary>
        /// Default <see cref="Term"/> value for the VROC indicator
        /// </summary>
        public const int DefaultVrocTerm = 14;

        /// <summary>
        /// The number of prices from previous days to include when computing 
        /// an indicator value
        /// </summary>
        public int Term { get; set; } = DefaultVrocTerm;

        /// <inheritdoc />
        [IgnoreIndicatorProperty]
        public IndicatorType Type => IndicatorType.Vroc;

        /// <inheritdoc />
        [IgnoreIndicatorProperty]
        public int RequiredPricesForSignalCount => Term;

        /// <inheritdoc />
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

        /// <inheritdoc />
        public IList<Signal> GenerateSignals(IList<Price> prices)
        {
            var signals = new List<Signal>();
            var values = Calculate(prices);
            var priceTrend = MovingAverageHelper.ExpotentialMovingAverage(prices, Term);
            for (int i = Term; i < prices.Count; i++)
            {
                if (prices[i].ClosePrice > priceTrend[i - Term + 1].Value && priceTrend[i - Term].Value < priceTrend[i - Term + 1].Value && values[i - Term].Value < 0)
                    signals.Add(new Signal(SignalAction.Buy) { Date = prices[i].Date });
                else if (prices[i].ClosePrice < priceTrend[i - Term + 1].Value && priceTrend[i - Term].Value > priceTrend[i - Term + 1].Value && values[i - Term].Value > 0)
                    signals.Add(new Signal(SignalAction.Sell) { Date = prices[i].Date });
            }
            return signals;
        }
    }
}