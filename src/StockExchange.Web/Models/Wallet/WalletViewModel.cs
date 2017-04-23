using StockExchange.Business.Models.Transaction;
using StockExchange.Business.Models.Wallet;
using System.Collections.Generic;

namespace StockExchange.Web.Models.Wallet
{
    /// <summary>
    /// View model for the Wallet view
    /// </summary>
    public class WalletViewModel
    {
        /// <summary>
        /// View model for adding transaction
        /// </summary>
        public AddTransactionViewModel AddTransactionViewModel { get; set; }

        /// <summary>
        /// User transactions
        /// </summary>
        public IList<UserTransactionDto> Transactions { get; set; } = new List<UserTransactionDto>();

        /// <summary>
        /// Currently owned stocks
        /// </summary>
        public IList<OwnedCompanyStocksDto> CurrentTransactions { get; set; } = new List<OwnedCompanyStocksDto>();

        /// <summary>
        /// Information about user's budget
        /// </summary>
        public BudgetInfoViewModel BudgetInfo { get; set; }
    }
}