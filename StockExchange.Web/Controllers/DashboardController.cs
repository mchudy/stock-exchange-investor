using StockExchange.Business.Extensions;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Paging;
using StockExchange.Business.Models.Wallet;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Helpers;
using StockExchange.Web.Helpers.Json;
using StockExchange.Web.Models.Charts;
using StockExchange.Web.Models.Dashboard;
using StockExchange.Web.Models.DataTables;
using StockExchange.Web.Models.Wallet;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StockExchange.Web.Controllers
{
    /// <summary>
    /// Controller for the user dashboard
    /// </summary>
    [Authorize]
    public class DashboardController : BaseController
    {
        private readonly ITransactionsService _transactionsService;
        private readonly IWalletService _walletService;
        private readonly IIndicatorsService _indicatorsService;
        private readonly IPriceService _priceService;

        /// <summary>
        /// Creates a new instance of <see cref="DashboardController"/>
        /// </summary>
        /// <param name="transactionsService"></param>
        /// <param name="walletService"></param>
        /// <param name="indicatorsService"></param>
        /// <param name="priceService"></param>
        public DashboardController(ITransactionsService transactionsService, IWalletService walletService, IIndicatorsService indicatorsService, IPriceService priceService)
        {
            _transactionsService = transactionsService;
            _walletService = walletService;
            _indicatorsService = indicatorsService;
            _priceService = priceService;
        }

        /// <summary>
        /// Returns the index view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var ownedStocks = await _walletService.GetOwnedStocks(CurrentUserId);
            var walletModel = await BuildWalletViewModel(ownedStocks);
            return View(walletModel);
        }

        /// <summary>
        /// Returns the data for the owned stocks table
        /// </summary>
        /// <param name="dataTableMessage"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetOwnedStocksTable(DataTableMessage<TransactionFilter> dataTableMessage)
        {
            var searchMessage = DataTableMessageConverter.ToPagedFilterDefinition(dataTableMessage);
            var pagedList = await _walletService.GetOwnedStocks(CurrentUserId, searchMessage);
            var model = GetSimpleDataTableResponse(dataTableMessage, pagedList);
            return new JsonNetResult(model, false);
        }

        /// <summary>
        /// Returns the data for the todays signals table
        /// </summary>
        /// <param name="dataTableMessage"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetTodaySignalsTable(DataTableMessage<TransactionFilter> dataTableMessage)
        {
            var searchMessage = DataTableMessageConverter.ToPagedFilterDefinition(dataTableMessage);
            var pagedList = await _indicatorsService.GetCurrentSignals(searchMessage);
            var model = GetSimpleDataTableResponse(dataTableMessage, pagedList);
            return new JsonNetResult(model, false);
        }

        /// <summary>
        /// Returns the data for the advancers table
        /// </summary>
        /// <param name="dataTableMessage"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetAdvancersTable(DataTableMessage<TransactionFilter> dataTableMessage)
        {
            var searchMessage = DataTableMessageConverter.ToPagedFilterDefinition(dataTableMessage);
            var pagedList = await _priceService.GetAdvancers(searchMessage);
            var model = GetSimpleDataTableResponse(dataTableMessage, pagedList);
            return new JsonNetResult(model, false);
        }

        /// <summary>
        /// Returns the data for the decliners table
        /// </summary>
        /// <param name="dataTableMessage"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetDeclinersTable(DataTableMessage<TransactionFilter> dataTableMessage)
        {
            var searchMessage = DataTableMessageConverter.ToPagedFilterDefinition(dataTableMessage);
            var pagedList = await _priceService.GetDecliners(searchMessage);
            var model = GetSimpleDataTableResponse(dataTableMessage, pagedList);
            return new JsonNetResult(model, false);
        }

        /// <summary>
        /// Returns the data for the most active companies table
        /// </summary>
        /// <param name="dataTableMessage"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetMostActiveTable(DataTableMessage<TransactionFilter> dataTableMessage)
        {
            var searchMessage = DataTableMessageConverter.ToPagedFilterDefinition(dataTableMessage);
            var pagedList = await _priceService.GetMostActive(searchMessage);
            var model = GetSimpleDataTableResponse(dataTableMessage, pagedList);
            return new JsonNetResult(model, false);
        }

        private static DataTableResponse<T> GetSimpleDataTableResponse<T>(DataTableMessage<TransactionFilter> dataTableMessage, PagedList<T> pagedList)
        {
            var model = new DataTableResponse<T>
            {
                RecordsFiltered = pagedList.TotalCount,
                RecordsTotal = pagedList.TotalCount,
                Data = pagedList,
                Draw = dataTableMessage.Draw
            };
            return model;
        }

        private async Task<DashboardViewModel> BuildWalletViewModel(IList<OwnedCompanyStocksDto> ownedStocks)
        {
            var data =
                ownedStocks.Select(g => new PieChartEntry {Name = g.CompanyName, Value = g.CurrentValue}).ToList();
            data.Add(new PieChartEntry
            {
                Name = "Free Budget",
                Value = CurrentUser.Budget
            });
            var walletModel = new DashboardViewModel
            {
                BudgetInfo = new BudgetInfoViewModel
                {
                    AllStocksValue = ownedStocks.Sum(s => s.CurrentValue),
                    FreeBudget = CurrentUser.Budget
                },
                AllTransactionsCount = await _transactionsService.GetTransactionsCount(CurrentUserId),
                OwnedCompanyStocks = ownedStocks,
                StocksByValue = new PieChartModel
                {
                    Title = "Owned stocks by value (PLN)",
                    Data = data
                },
                CurrentSignalsCount = await _indicatorsService.GetCurrentSignalsCount()
            };
            return walletModel;
        }
    }
}