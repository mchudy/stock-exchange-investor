using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;

namespace StockExchange.Business.Indicators
{
    /// <summary>
    /// Volume Price Trend technical indicator
    /// </summary>
    [StrategyIgnoreIndicator]
    [IndicatorDescription("Vpt")]
    public class VptIndicator : IIndicator
    {
        private const int _priceTerm = 14;

        /// <inheritdoc />
        [IgnoreIndicatorProperty]
        public IndicatorType Type => IndicatorType.Vpt;

        /// <inheritdoc />
        [IgnoreIndicatorProperty]
        public int RequiredPricesForSignalCount => _priceTerm + 1;

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
            return new List<Signal>();
        }
    }
}
