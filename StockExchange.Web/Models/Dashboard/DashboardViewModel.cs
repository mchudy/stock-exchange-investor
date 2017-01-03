using System.Collections.Generic;
using StockExchange.Business.Models.Wallet;
using StockExchange.Web.Models.Charts;
using StockExchange.Web.Models.Wallet;

namespace StockExchange.Web.Models.Dashboard
{
    public class DashboardViewModel
    {
        public BudgetInfoViewModel BudgetInfo { get; set; }

        public int AllTransactionsCount { get; set; }

        public int CurrentSignalsCount { get; set; }

        public IList<OwnedCompanyStocksDto> OwnedCompanyStocks { get; set; } = new List<OwnedCompanyStocksDto>();

        public PieChartModel StocksByValue { get; set; }


        public string CurrentStrategyName { get; set; }
    }
}