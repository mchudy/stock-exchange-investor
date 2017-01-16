using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Business.Models.Indicators
{
    /// <summary>
    /// Class containing indicator descriptions.
    /// </summary>
    public class IndicatorDescriptionDto
    {
        /// <summary>
        /// Indicator description.
        /// </summary>
        public string IndicatorDescription { get; set; }

        /// <summary>
        /// Buy signal description. 
        /// </summary>
        public string BuySignalDescription { get; set; }

        /// <summary>
        /// Sell signal description.
        /// </summary>
        public string SellSignalDescription { get; set; }
    }
}
