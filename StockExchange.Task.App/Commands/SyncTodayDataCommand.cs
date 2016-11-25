using log4net;
using StockExchange.Common;
using StockExchange.Task.App.Helpers;
using StockExchange.Task.Business;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace StockExchange.Task.App.Commands
{
    [CommandName(Consts.Commands.SyncTodayData, "Synchronizes data from the last day")]
    internal class SyncTodayDataCommand : ICommand
    {
        private readonly IDataSynchronizer _synchronizer;
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public SyncTodayDataCommand(IDataSynchronizer synchronizer)
        {
            _synchronizer = synchronizer;
        }

        public void Execute(IEnumerable<string> parameters)
        {
            var startDate = DateTime.Now.AddDays(-1).Date;
            var endDate = DateTime.Now.Date;
            Logger.Debug($"Synchronizing data from {startDate.ToShortDateString()}");
            _synchronizer.Sync(startDate, endDate);
        }
    }
}
