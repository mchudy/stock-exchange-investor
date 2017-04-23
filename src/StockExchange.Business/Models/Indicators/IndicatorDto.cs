using StockExchange.Business.Indicators.Common;

namespace StockExchange.Business.Models.Indicators
{
    /// <summary>
    /// Represents a technical indicator
    /// </summary>
    public class IndicatorDto
    {
        /// <summary>
        /// The indicator type
        /// </summary>
        public IndicatorType IndicatorType { get; set; }

        /// <summary>
        /// The indicator name
        /// </summary>
        public string IndicatorName { get; set; }
    }
}
