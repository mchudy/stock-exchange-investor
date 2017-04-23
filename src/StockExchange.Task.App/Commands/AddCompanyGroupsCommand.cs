using StockExchange.Common;
using StockExchange.Task.App.Helpers;
using StockExchange.Task.Business;
using System.Collections.Generic;

namespace StockExchange.Task.App.Commands
{
    [CommandName(Consts.Commands.AddCompanyGroups, "Adds company groups to the database")]
    internal class AddCompanyGroupsCommand : ICommand
    {
        private readonly ICompanyGroupsSynchronizer _companyGroupsSynchronizer;

        public AddCompanyGroupsCommand(ICompanyGroupsSynchronizer companyGroupsSynchronizer)
        {
            _companyGroupsSynchronizer = companyGroupsSynchronizer;
        }

        public void Execute(IEnumerable<string> parameters)
        {
            System.Threading.Tasks.Task.Run(() =>_companyGroupsSynchronizer.UpdateCompanyGroups()).Wait();
        }
    }
}
