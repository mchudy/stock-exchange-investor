using System;
using System.Collections.Generic;

namespace StockExchange.DataAccess.Models
{
    public class Simulation
    {
        public int Id { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int StrategyId { get; set; }
        public virtual InvestmentStrategy Strategy { get; set; }

        public virtual ICollection<SimulationCompany> SimultionCompanies { get; set; } = new HashSet<SimulationCompany>();
    }
}
