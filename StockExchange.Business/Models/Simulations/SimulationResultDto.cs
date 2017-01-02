using System;
using System.Collections.Generic;

namespace StockExchange.Business.Models.Simulations
{
    public class ExtremeTransactionResult
    {
        public decimal ValueBefore { get; set; }
        public decimal ValueAfter { get; set; }
        public DateTime TransactionDate { get; set; }

        public double PercentageIncome => ValueBefore != 0 ? Math.Round((double)((ValueAfter - ValueBefore) / ValueBefore), 2) : 0.0;

        public ExtremeTransactionResult(DateTime transactionDate, decimal valueBefore, decimal valueAfter)
        {
            TransactionDate = transactionDate;
            ValueBefore = valueBefore;
            ValueAfter = valueAfter;
        }
    }

    public class SimulationResultDto
    {
        public IList<SimulationTransactionDto> TransactionsLog { get; set; }

        public Dictionary<int, int> CurrentCompanyQuantity { get; set; }

        public decimal StartBudget { get; set; }
        public decimal SimulationTotalValue { get; set; }
        public double PercentageProfit { get; set; }

        public ExtremeTransactionResult MaximalGainOnTransaction { get; set; }
        public ExtremeTransactionResult MaximalLossOnTransaction { get; set; }
    }
}
