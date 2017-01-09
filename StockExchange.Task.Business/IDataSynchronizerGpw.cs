using System;

namespace StockExchange.Task.Business
{
    /// <summary>
    /// Synchronizes stock data using the GPW sources
    /// </summary>
    public interface IDataSynchronizerGpw
    {
        /// <summary>
        /// Synchronizes the data from the given date
        /// </summary>
        /// <param name="date">Date for which to synchronize the stock data</param>
        void Sync(DateTime date);
    }
}
