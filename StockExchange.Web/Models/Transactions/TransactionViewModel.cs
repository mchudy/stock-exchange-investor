using System.Collections.Generic;
using StockExchange.Business.Models.Transaction;

namespace StockExchange.Web.Models.Transactions
{
    public class TransactionViewModel
    {
        public AddTransactionViewModel AddTransactionViewModel { get; set; }

        public IList<UserTransactionDto> Transactions { get; set; }
    }
}