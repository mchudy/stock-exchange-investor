using System;
using System.Collections.Generic;
using System.Linq;
using StockExchange.Common;
using StockExchange.Task.App.Helpers;
using StockExchange.Task.Business;

namespace StockExchange.Task.App.Commands
{
    [CommandName(Consts.Commands.SyncDataGpw, "Synchronizes historical data")]
    internal sealed class SyncHistoricalDataGpwCommand : ICommand
    {
        private readonly IDataSynchronizerGpw _synchronizer;

        public SyncHistoricalDataGpwCommand(IDataSynchronizerGpw synchronizer)
        {
            _synchronizer = synchronizer;
        }

        public void Execute(IEnumerable<string> parameters)
        {
            var arrayParameters = parameters as string[] ?? parameters.ToArray();
            var startDate = arrayParameters.Any() 
                ? DateTime.ParseExact(arrayParameters[0], Consts.Formats.DateFormat, System.Globalization.CultureInfo.InvariantCulture) 
                : DateTime.Parse(Consts.SyncDataParameters.StartDate);
            var endDate = arrayParameters.Length > 1 
                ? DateTime.ParseExact(arrayParameters[1], Consts.Formats.DateFormat, System.Globalization.CultureInfo.InvariantCulture)
                : DateTime.Now;
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                _synchronizer.Sync(date);
            }
        }
    }
}
