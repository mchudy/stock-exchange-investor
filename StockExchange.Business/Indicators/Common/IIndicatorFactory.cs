using System.Collections.Generic;

namespace StockExchange.Business.Indicators.Common
{
    /// <summary>
    /// Provides methods to create <see cref="IIndicator"/> instances
    /// </summary>
    public interface IIndicatorFactory
    {
        /// <summary>
        /// Creates a new indicator of the given type with default parameters
        /// </summary>
        /// <param name="indicatorType">The type of indicator to be created</param>
        /// <returns>The new indicator</returns>
        IIndicator CreateIndicator(IndicatorType indicatorType);

        /// <summary>
        /// Creates a new indicator of the given with the given parameters
        /// </summary>
        /// <param name="indicatorType">The type of indicator to be created</param>
        /// <param name="parameters">The indicator parameters</param>
        /// <returns>The new indicator</returns>
        IIndicator CreateIndicator(IndicatorType indicatorType, Dictionary<string, int> parameters);
    }
}