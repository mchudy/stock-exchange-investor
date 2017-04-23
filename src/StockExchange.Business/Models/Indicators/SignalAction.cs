namespace StockExchange.Business.Models.Indicators
{
    /// <summary>
    /// Action to take indicated by the signal
    /// </summary>
    public enum SignalAction
    {
        /// <summary>
        /// The stocks should be bought
        /// </summary>
        Buy,

        /// <summary>
        /// The stocks should be sold
        /// </summary>
        Sell,

        /// <summary>
        /// No action should be taken
        /// </summary>
        NoSignal
    }
}