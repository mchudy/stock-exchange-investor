using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;

namespace StockExchange.Business.Indicators
{
    /// <summary>
    /// OBV technical indicator
    /// </summary>
    [StrategyIgnoreIndicator]
    [IndicatorDescription("Obv")]
    public class ObvIndicator : IIndicator
    {
        private const int _trendTerm = 20;

        /// <inheritdoc />
        [IgnoreIndicatorProperty]
        public IndicatorType Type => IndicatorType.Obv;

        /// <inheritdoc />
        [IgnoreIndicatorProperty]
        public int RequiredPricesForSignalCount => _trendTerm;

        /// <inheritdoc />
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

        /// <inheritdoc />
        public IList<Signal> GenerateSignals(IList<Price> prices)
        {
            return new List<Signal>(); //throw exception?
        }
    }
}
