using System.Collections.Generic;
using System.Reflection;
using log4net;
using StockExchange.Task.App.Helpers;

namespace StockExchange.Task.App.Commands
{
    [CommandName("help", "Displays help")]
    internal sealed class HelpCommand : ICommand
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(IEnumerable<string> parameters)
        {
            Logger.Debug("Available commands:");
            foreach (var command in CommandHelper.GetAvailableCommandNames(Assembly.GetExecutingAssembly()))
            {
                Logger.Debug(command.Name + " - " + command.Description);
            }
        }
    }
}
