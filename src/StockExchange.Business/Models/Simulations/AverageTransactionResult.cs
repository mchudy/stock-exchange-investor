using System;

namespace StockExchange.Business.Models.Simulations
{
    /// <summary>
    /// Represents an average transaction result
    /// </summary>
    public class AverageTransactionResult
    {
        /// <summary>
        /// Creates a new instance of <see cref="AverageTransactionResult"/>
        /// </summary>
        /// <param name="value">Value of all transactions</param>
        /// <param name="transactionCount">Transaction count</param>
        public AverageTransactionResult(decimal value, int transactionCount)
        {
            Value = transactionCount == 0 ? 0 : Math.Round(value / transactionCount, 2);
            TransactionCount = transactionCount;
        }

        /// <summary>
        /// Value of all transactions
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Transaction count.
        /// </summary>
        public int TransactionCount { get; set; }
    }
}