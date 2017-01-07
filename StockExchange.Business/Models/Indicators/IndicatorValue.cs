using System;

namespace StockExchange.Business.Models.Indicators
{
    /// <summary>
    /// Represents an indicator value
    /// </summary>
    public class IndicatorValue
    {
        /// <summary>
        /// The date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The indicator value
        /// </summary>
        public decimal Value { get; set; }
    }

    /// <summary>
    /// Represents an indicator value which contains two lines (e.g. MACD)
    /// </summary>
    public class DoubleLineIndicatorValue : IndicatorValue
    {
        /// <summary>
        /// Indicator value for the second line
        /// </summary>
        public decimal SecondLineValue { get; set; }
    }
}
