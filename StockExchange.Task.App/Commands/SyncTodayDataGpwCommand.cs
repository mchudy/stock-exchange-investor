using log4net;
using StockExchange.Common;
using StockExchange.Task.App.Helpers;
using StockExchange.Task.Business;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace StockExchange.Task.App.Commands
{
    /// <summary>
    /// Synchronizes today's stock data with the database using GPW sources
    /// </summary>
    [CommandName(Consts.Commands.SyncTodayDataGpw, "Synchronizes data from the last day")]
    internal class SyncTodayDataGpwCommand : ICommand
    {
        private readonly IDataSynchronizerGpw _synchronizer;
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public SyncTodayDataGpwCommand(IDataSynchronizerGpw synchronizer)
        {
            _synchronizer = synchronizer;
        }

        public void Execute(IEnumerable<string> parameters)
        {
            var date = DateTime.Now.Date;
            Logger.Debug($"Synchronizing data from {date.ToShortDateString()}");
            System.Threading.Tasks.Task.Run(() => _synchronizer.Sync(date)).Wait();
        }
    }
}
