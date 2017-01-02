using StockExchange.Business.Models.Transaction;
using StockExchange.Business.Models.Wallet;
using StockExchange.Web.Models.Wallet;
using System.Collections.Generic;

namespace StockExchange.Web.Models.Transactions
{
    public class WalletViewModel
    {
        public AddTransactionViewModel AddTransactionViewModel { get; set; }

        public IList<UserTransactionDto> Transactions { get; set; } = new List<UserTransactionDto>();

        public IList<OwnedCompanyStocksDto> CurrentTransactions { get; set; } = new List<OwnedCompanyStocksDto>();

        public BudgetInfoViewModel BudgetInfo { get; set; }
    }
}