using System;

namespace StockExchange.Business.Models.Simulations
{
    /// <summary>
    /// Represents an extreme transaction result
    /// </summary>
    public class ExtremeTransactionResult
    {
        /// <summary>
        /// Creates a new instance of <see cref="ExtremeTransactionResult"/>
        /// </summary>
        /// <param name="transactionDate">The date when the transaction was concluded</param>
        /// <param name="valueBefore">Value before the transaction</param>
        /// <param name="valueAfter">Value after the transaction</param>
        public ExtremeTransactionResult(DateTime transactionDate, decimal valueBefore, decimal valueAfter)
        {
            TransactionDate = transactionDate;
            ValueBefore = valueBefore;
            ValueAfter = valueAfter;
        }

        /// <summary>
        /// Value before the transaction
        /// </summary>
        public decimal ValueBefore { get; set; }

        /// <summary>
        /// Value after the transaction
        /// </summary>
        public decimal ValueAfter { get; set; }

        /// <summary>
        /// The date when the transaction was concluded
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// The percentage income on the transaction
        /// </summary>
        public double PercentageIncome => ValueBefore != 0 ? Math.Round((double)((ValueAfter - ValueBefore) / ValueBefore), 2) : 0.0;
    }
}