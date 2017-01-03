using System.Collections.Generic;
using StockExchange.Business.Models.Company;
using StockExchange.Business.Models.Simulations;

namespace StockExchange.Web.Models.Simulation
{
    public class SimulationResultViewModel
    {
        public IList<SimulationTransaction> TransactionsLog { get; set; }

        public Dictionary<CompanyDto, int> CurrentCompanyQuantity { get; set; }

        public decimal StartBudget { get; set; }
        public decimal TotalSimulationValue { get; set; }
        public double PercentageProfit { get; set; }
        public ExtremeTransactionResult MaximalGainOnTransaction { get; set; }
        public ExtremeTransactionResult MaximalLossOnTransaction { get; set; }
        public ExtremeSimulationValue MaximalSimulationValue { get; set; }
        public ExtremeSimulationValue MinimalSimulationValue { get; set; }
    }
}
