using StockExchange.Business.Models.Wallet;
using StockExchange.Web.Models.Charts;
using System.Collections.Generic;

namespace StockExchange.Web.Models.Wallet
{
    public class WalletViewModel
    {
        public BudgetInfoViewModel BudgetInfo { get; set; }

        public int AllTransactionsCount { get; set; }

        public int CurrentSignalsCount { get; set; }

        public string CurrentStrategyName { get; set; }

        public IList<OwnedCompanyStocksDto> OwnedCompanyStocks { get; set; } = new List<OwnedCompanyStocksDto>();

        public PieChartModel StocksByValue { get; set; }
    }
}