using StockExchange.Business.Models;
using System.Collections.Generic;

namespace StockExchange.Web.Models.Wallet
{
    public class WalletViewModel
    {
        public decimal FreeBudget { get; set; }
        public decimal AllStocksValue { get; set; }
        public decimal TotalBudget => FreeBudget + AllStocksValue;
        
        public int AllTransactionsCount { get; set; }
        public int CurrentSignalsCount { get; set; }

        public IList<OwnedCompanyStocksDto> OwnedCompanyStocks { get; set; } = new List<OwnedCompanyStocksDto>();
    }
}