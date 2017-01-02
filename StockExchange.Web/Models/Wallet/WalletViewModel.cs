using System.Collections.Generic;
using StockExchange.Business.Models.Transaction;
using StockExchange.Business.Models.Wallet;

namespace StockExchange.Web.Models.Wallet
{
    public class WalletViewModel
    {
        public AddTransactionViewModel AddTransactionViewModel { get; set; }

        public IList<UserTransactionDto> Transactions { get; set; } = new List<UserTransactionDto>();

        public IList<OwnedCompanyStocksDto> CurrentTransactions { get; set; } = new List<OwnedCompanyStocksDto>();

        public BudgetInfoViewModel BudgetInfo { get; set; }
    }
}