using System.Collections.Generic;

namespace StockExchange.Task.App.Commands
{
    /// <summary>
    /// A command which can be executed by the application
    /// </summary>
    internal interface ICommand
    {
        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="parameters">Command parameters</param>
        void Execute(IEnumerable<string> parameters);
    }
}