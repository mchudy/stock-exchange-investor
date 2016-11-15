using System;

namespace StockExchange.Task.Business
{
    public interface IDataSynchronizerGpw
    {
        void Sync(DateTime date);
    }
}
