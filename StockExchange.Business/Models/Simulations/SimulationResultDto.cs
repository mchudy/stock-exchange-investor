using System.Collections.Generic;

namespace StockExchange.Business.Models.Simulations
{
    /// <summary>
    /// Represents the result of a simulation
    /// </summary>
    public class SimulationResultDto
    {
        /// <summary>
        /// The transactions concluded during the simulation
        /// </summary>
        public IList<SimulationTransactionDto> TransactionsLog { get; set; }

        /// <summary>
        /// Number of stocks owned at the end of the simulation
        /// </summary>
        public Dictionary<int, int> CurrentCompanyQuantity { get; set; }

        /// <summary>
        /// The start budget
        /// </summary>
        public decimal StartBudget { get; set; }

        /// <summary>
        /// The end budget
        /// </summary>
        public decimal SimulationTotalValue { get; set; }

        /// <summary>
        /// The percentage profit at the end of the simulation
        /// </summary>
        public double PercentageProfit { get; set; }

        /// <summary>
        /// The minimal simulation value achieved during the simulation
        /// </summary>
        public ExtremeSimulationValue MinimalSimulationValue { get; set; }

        /// <summary>
        /// The maximal simulation value achieved during the simulation
        /// </summary>
        public ExtremeSimulationValue MaximalSimulationValue { get; set; }

        /// <summary>
        /// Transaction statistics.
        /// </summary>
        public TransactionStatistics TransactionStatistics { get; set; }
    }
}
