using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;

namespace StockExchange.Business.Indicators.Common
{
    /// <summary>
    /// Represents a technical indicator
    /// </summary>
    public interface IIndicator
    {
        /// <summary>
        /// Type of the indicator
        /// </summary>
        [IgnoreIndicatorProperty]
        IndicatorType Type { get; }

        /// <summary>
        /// Gets the number of days in which indicator values are not defined.
        /// </summary>
        [IgnoreIndicatorProperty]
        int RequiredPricesForSignalCount { get; }

        /// <summary>
        /// Calculates the indicator for the given prices
        /// </summary>
        /// <param name="prices">Prices for which the indicator values should be calculated</param>
        /// <returns>A list of computed indicator values</returns>
        IList<IndicatorValue> Calculate(IList<Price> prices);

        /// <summary>
        /// Returns a list of signals which the indicator generated for the given prices
        /// </summary>
        /// <param name="prices">Prices for which the signals should be found</param>
        /// <returns>A list of found signals</returns>
        IList<Signal> GenerateSignals(IList<Price> prices);
    }
}
