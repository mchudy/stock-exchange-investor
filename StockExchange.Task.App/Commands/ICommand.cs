using System.Collections.Generic;

namespace StockExchange.Task.App.Commands
{
    internal interface ICommand
    {
        void Execute(IEnumerable<string> parameters);
    }
}