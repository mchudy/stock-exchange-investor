using System.Collections.Generic;
using StockExchange.Business.Models.Company;

namespace StockExchange.Web.Models.Simulation
{
    public class SimulationResultViewModel
    {
        public IList<SimulationTransaction> TransactionsLog { get; set; }

        public Dictionary<CompanyDto, int> CurrentCompanyQuantity { get; set; }
    }
}
