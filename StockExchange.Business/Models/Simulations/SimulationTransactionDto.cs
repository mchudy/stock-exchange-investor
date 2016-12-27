using System;
using StockExchange.Business.Models.Indicators;

namespace StockExchange.Business.Models.Simulations
{
    public class SimulationTransactionDto
    {
        public DateTime Date { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int CompanyId { get; set; }

        public decimal BudgetAfter { get; set; }

        public SignalAction Action { get; set; }
    }
}
