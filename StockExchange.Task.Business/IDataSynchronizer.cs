using System;
using System.Collections.Generic;

namespace StockExchange.Task.Business
{
    public interface IDataSynchronizer
    {
        void Sync(DateTime startDate, DateTime endDate, IEnumerable<string> companyCodes = null);
    }
}
