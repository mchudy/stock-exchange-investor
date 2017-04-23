namespace StockExchange.Business.Models.Simulations
{
    /// <summary>
    /// Contains some statistics about transactions gains and losses
    /// </summary>
    public class TransactionStatistics
    {
        /// <summary>
        /// The average gain on a single transaction achieved during the simulation
        /// </summary>
        public AverageTransactionResult AverageGainOnTransaction { get; set; }

        /// <summary>
        /// The average loss on a single transaction achieved during the simulation
        /// </summary>
        public AverageTransactionResult AverageLossOnTransaction { get; set; }

        /// <summary>
        /// Success transaction percentage.
        /// </summary>
        public double SuccessTransactionPercentage { get; set; }

        /// <summary>
        /// Failed transaction percentage.
        /// </summary>
        public double FailedTransactionPercentage { get; set; }
    }
}