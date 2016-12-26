using System;
using System.Collections.Generic;

namespace StockExchange.Business.Models.Simulations
{
    public class SimulationDto
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public IList<int> SelectedCompanyIds { get; set; }

        public int SelectedStrategyId { get; set; }

        public int UserId { get; set; }
    }
}
