using StockExchange.Business.Models.Indicators;
using StockExchange.Business.Models.Price;
using StockExchange.Business.Models.Wallet;
using StockExchange.Web.Models.Charts;
using StockExchange.Web.Models.Wallet;
using System.Collections.Generic;

namespace StockExchange.Web.Models.Dashboard
{
    /// <summary>
    /// View model for the dashboard view
    /// </summary>
    public class DashboardViewModel
    {
        /// <summary>
        /// Information about current user budget
        /// </summary>
        public BudgetInfoViewModel BudgetInfo { get; set; }

        /// <summary>
        /// Number of all transactions concluded by the user
        /// </summary>
        public int AllTransactionsCount { get; set; }

        /// <summary>
        /// Number of signals generated for today's prices
        /// </summary>
        public int CurrentSignalsCount { get; set; }

        /// <summary>
        /// Stocks owned by the current user
        /// </summary>
        public IList<OwnedCompanyStocksDto> OwnedCompanyStocks { get; set; } = new List<OwnedCompanyStocksDto>();

        /// <summary>
        /// Pie chart data for owned stocks
        /// </summary>
        public PieChartModel StocksByValue { get; set; }

        /// <summary>
        /// List of today's signals
        /// </summary>
        public IList<TodaySignal> TodaySignals { get; set; }

        /// <summary>
        /// List of companies with the highest gain
        /// </summary>
        public IList<MostActivePriceDto> Advancers { get; set; }

        /// <summary>
        /// List of the companies with the highest losses
        /// </summary>
        public IList<MostActivePriceDto> Decliners { get; set; }

        /// <summary>
        /// List of the most active companies (by volume)
        /// </summary>
        public IList<MostActivePriceDto> MostActive { get; set; }
    }
}