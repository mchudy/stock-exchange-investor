﻿using StockExchange.Business.Indicators.Common;
using System.Collections.Generic;
using StockExchange.Business.Models.Indicators;

namespace StockExchange.Web.Models.Indicator
{
    /// <summary>
    /// View model for editing an indicator
    /// </summary>
    public class EditIndicatorViewModel
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

        /// <summary>
        /// Whether the indicator has been selected
        /// </summary>
        public bool IsSelected { get; set; }

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