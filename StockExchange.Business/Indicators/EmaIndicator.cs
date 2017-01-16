using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;

namespace StockExchange.Business.Indicators
{
    /// <summary>
    /// Exponential moving average technical indicator
    /// </summary>
    public class EmaIndicator : IIndicator
    {
        /// <summary>
        /// Default <see cref="Term"/> value for the EMA indicator
        /// </summary>
        public const int DefaultTerm = 5;

        /// <summary>
        /// The number of prices from previous days to include when computing 
        /// an indicator value
        /// </summary>
        public int Term { get; set; } = DefaultTerm;

        /// <inheritdoc />
        [IngoreIndicatorProperty]
        public IndicatorType Type => IndicatorType.Ema;

        /// <inheritdoc />
        [IngoreIndicatorProperty]
        public int RequiredPricesCountToSignal => Term;

        /// <inheritdoc />
        public IList<IndicatorValue> Calculate(IList<Price> prices)
        {
            return MovingAverageHelper.ExpotentialMovingAverage(prices, Term);
        }

        /// <inheritdoc />
        public IList<Signal> GenerateSignals(IList<Price> prices)
        {
            return MovingAverageHelper.GenerateSignalsForMovingAverages(this, Term, prices);
        }
    }
}
