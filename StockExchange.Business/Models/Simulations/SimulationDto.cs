using System;
using System.Collections.Generic;

namespace StockExchange.Business.Models.Simulations
{
    /// <summary>
    /// Represents a trading game simulation
    /// </summary>
    public class SimulationDto
    {
        /// <summary>
        /// Start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Ids of companies to include in the simulation
        /// </summary>
        public IList<int> SelectedCompanyIds { get; set; }

        /// <summary>
        /// Id of the selected strategy
        /// </summary>
        public int SelectedStrategyId { get; set; }

        /// <summary>
        /// Id of the user performing the simulation
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Total budget for the simulation
        /// </summary>
        public decimal Budget { get; set; }

        /// <summary>
        /// Maximal budget for single buy transaction.
        /// </summary>
        public decimal MaximalBudgetPerTransaction { get; set; }

        /// <summary>
        /// Indicates whether transaction limit is set. 
        /// </summary>
        public bool HasTransactionLimit { get; set; }
    }
}
