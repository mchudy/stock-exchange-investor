using System.Collections.Generic;
using StockExchange.Business.Models.Transaction;
using StockExchange.Business.Models.Wallet;

namespace StockExchange.Web.Models.Transactions
{
    public class TransactionViewModel
    {
        public AddTransactionViewModel AddTransactionViewModel { get; set; }

        public IList<UserTransactionDto> Transactions { get; set; }

        public IList<OwnedCompanyStocksDto> CurrentTransactions { get; set; }
    }
}