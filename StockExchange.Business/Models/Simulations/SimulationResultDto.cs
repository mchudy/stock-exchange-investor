using System.Collections.Generic;

namespace StockExchange.Business.Models.Simulations
{
    public class SimulationResultDto
    {
        public IList<SimulationTransactionDto> TransactionsLog { get; set; }

        public Dictionary<int, int> CurrentCompanyQuantity { get; set; }
    }
}
