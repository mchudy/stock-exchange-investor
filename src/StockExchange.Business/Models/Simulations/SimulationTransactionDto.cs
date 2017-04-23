using StockExchange.Business.Models.Indicators;
using System;

namespace StockExchange.Business.Models.Simulations
{
    /// <summary>
    /// Represents a transaction concluded during the simulation
    /// </summary>
    public class SimulationTransactionDto
    {
        /// <summary>
        /// The date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The number of stocks involved
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The company id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// The total budget after concluding the transaction
        /// </summary>
        public decimal BudgetAfter { get; set; }

        /// <summary>
        /// The total budget after concluding the transaction
        /// </summary>
        public decimal Profit { get; set; }

        /// <summary>
        /// The action taken
        /// </summary>
        public SignalAction Action { get; set; }
    }
}
