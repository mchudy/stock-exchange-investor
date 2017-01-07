using StockExchange.Common;
using StockExchange.Task.App.Helpers;
using StockExchange.Task.Business;
using System.Collections.Generic;

namespace StockExchange.Task.App.Commands
{
    /// <summary>
    /// Fixes incorrect data from the GPW sources. Should be run once after
    /// syncing all historical data.
    /// </summary>
    [CommandName(Consts.Commands.FixData, "Fixes incorrect data from GPW")]
    internal class FixDataCommand : ICommand
    {
        private readonly IDataFixer _dataFixer;

        internal FixDataCommand(IDataFixer dataFixer)
        {
            _dataFixer = dataFixer;
        }

        public void Execute(IEnumerable<string> parameters)
        {
            _dataFixer.FixData();
        }
    }
}
