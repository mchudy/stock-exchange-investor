using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Business.Indicators.Common
{
    /// <summary>
    /// Connects an indicator with its description.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class IndicatorDescriptionAttribute : Attribute
    {
        /// <summary>
        /// Buy signal description. 
        /// </summary>
        public string BuySignalDescription { get; }
        
        /// <summary>
        /// Sell signal description.
        /// </summary>
        public string SellSignalDescription { get; }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="indicatorName">Indicator name.</param>
        public IndicatorDescriptionAttribute(string indicatorName)
        {
            BuySignalDescription = IndicatorDescriptions.ResourceManager.GetString(indicatorName + BuySignalSuffix);
            SellSignalDescription = IndicatorDescriptions.ResourceManager.GetString(indicatorName + SellSignalSuffix);
        }

        private const string BuySignalSuffix = "BuySignal";
        private const string SellSignalSuffix = "SellSignal";
    }
}
