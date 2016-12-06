using System.Collections.Generic;
using StockExchange.DataAccess.Models;

namespace StockExchange.Business.Models
{
    public class SimulationResult
    {
        public int Id { get; set; }

        public Simulation Simulation { get; set; }

        public IList<SimulationValue> SimulationValues { get; set; }
        public IList<Transaction> Transactions { get; set; }
        public IList<CompanyStockQuantity> CompanyStockQuantities { get; set; }
    }
}
