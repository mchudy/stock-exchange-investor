using System;

namespace StockExchange.Task.Business
{
    public interface IHistoricalDataSynchronizer
    {
        void Sync(DateTime startDate, DateTime endDate, string companyCode = null);
    }
}
