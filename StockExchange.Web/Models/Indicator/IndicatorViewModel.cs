using StockExchange.Business.Indicators.Common;
using System.Collections.Generic;

namespace StockExchange.Web.Models.Indicator
{
    /// <summary>
    /// View model for a single indicator
    /// </summary>
    public class IndicatorViewModel
    {
        /// <summary>
        /// Indicator type
        /// </summary>
        public IndicatorType Type { get; set; }

        /// <summary>
        /// Indicator name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indicator properties
        /// </summary>
        public IList<IndicatorPropertyViewModel> Properties { get; set; } = new List<IndicatorPropertyViewModel>();

    }
}