namespace StockExchange.Task.Business
{
    /// <summary>
    /// Fixes data downloaded from the GPW sources
    /// </summary>
    public interface IDataFixer
    {
        /// <summary>
        /// Fixes the stock data
        /// </summary>
        void FixData();
    }
}