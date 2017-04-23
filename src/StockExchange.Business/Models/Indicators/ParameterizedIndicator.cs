using StockExchange.Business.Indicators.Common;
using System.Collections.Generic;

namespace StockExchange.Business.Models.Indicators
{
    /// <summary>
    /// Represents and indicator with custom properties
    /// </summary>
    public class ParameterizedIndicator
    {
        /// <summary>
        /// The indicator type
        /// </summary>
        public IndicatorType? IndicatorType { get; set; }

        /// <summary>
        /// The indicator properties
        /// </summary>
        public IList<IndicatorProperty> Properties { get; set; } 
    }
}
