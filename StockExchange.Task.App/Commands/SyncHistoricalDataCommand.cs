using System;
using System.Collections.Generic;
using StockExchange.Task.App.Helpers;
using StockExchange.Task.Business;

namespace StockExchange.Task.App.Commands
{
    [CommandName("sync-historical-data", "Synchronizes historical data")]
    internal sealed class SyncHistoricalDataCommand : ICommand
    {
        private readonly IHistoricalDataSynchronizer _synchronizer;

        public SyncHistoricalDataCommand(IHistoricalDataSynchronizer synchronizer)
        {
            _synchronizer = synchronizer;
        }

        public void Execute(IEnumerable<string> parameters)
        {
            _synchronizer.Sync(new DateTime(2006,01,01), DateTime.Today);
        }
    }
}
