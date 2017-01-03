using System.Collections.Generic;
using StockExchange.Business.Models.Indicators;
using StockExchange.Business.Models.Price;
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

        public IList<TodaySignal> TodaySignals { get; set; }

        public IList<MostActivePriceDto> Advancers { get; set; }

        public IList<MostActivePriceDto> Decliners { get; set; }

        public IList<MostActivePriceDto> MostActive { get; set; }
    }
}