using System;
using System.Collections.Generic;

namespace StockExchange.Task.Business
{
    /// <summary>
    /// Synchronizes stock data
    /// </summary>
    public interface IDataSynchronizer
    {
        /// <summary>
        /// Synchronizes the stock data
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <param name="companyCodes">Companies to synchronize</param>
        void Sync(DateTime startDate, DateTime endDate, IEnumerable<string> companyCodes = null);
    }
}
