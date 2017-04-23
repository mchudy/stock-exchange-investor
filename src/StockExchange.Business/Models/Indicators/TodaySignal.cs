namespace StockExchange.Business.Models.Indicators
{
    /// <summary>
    /// Represents a signal computed for the current day
    /// </summary>
    public class TodaySignal
    {
        /// <summary>
        /// Action to take
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Company which stocks to buy or sell
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Indicator which generated the signal
        /// </summary>
        public string Indicator { get; set; }
    }
}
