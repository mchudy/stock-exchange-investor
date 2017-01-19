namespace StockExchange.Business.Models.Simulations
{
    /// <summary>
    /// Contains some statistics about transactions gains and losses
    /// </summary>
    public class TransactionStatistics
    {
        /// <summary>
        /// The maximal gain on a single transaction achieved during the simulation
        /// </summary>
        public ExtremeTransactionResult MaximalGainOnTransaction { get; set; }

        /// <summary>
        /// The maximal loss on a single transaction achieved during the simulation
        /// </summary>
        public ExtremeTransactionResult MaximalLossOnTransaction { get; set; }

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