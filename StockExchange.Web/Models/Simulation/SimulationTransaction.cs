using System;
using StockExchange.Business.Models.Company;
using StockExchange.Business.Models.Indicators;

namespace StockExchange.Web.Models.Simulation
{
    public class SimulationTransaction
    {
        public DateTime Date { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public CompanyDto Company { get; set; }

        public decimal BudgetAfter { get; set; }

        public SignalAction Action { get; set; }
    }
}
