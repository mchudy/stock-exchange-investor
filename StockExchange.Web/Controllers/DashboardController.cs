﻿using StockExchange.Business.Extensions;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Indicators;
using StockExchange.Business.Models.Price;
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
    [Authorize]
    public class DashboardController : BaseController
    {
        private readonly ITransactionsService _transactionsService;
        private readonly IWalletService _walletService;
        private readonly IIndicatorsService _indicatorsService;
        private readonly IPriceService _priceService;

        public DashboardController(ITransactionsService transactionsService, IWalletService walletService, IIndicatorsService indicatorsService, IPriceService priceService)
        {
            _transactionsService = transactionsService;
            _walletService = walletService;
            _indicatorsService = indicatorsService;
            _priceService = priceService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var ownedStocks = await _walletService.GetOwnedStocks(CurrentUserId);
            var walletModel = await BuildWalletViewModel(ownedStocks);
            return View(walletModel);
        }

        [HttpPost]
        public async Task<ActionResult> GetOwnedStocksTable(DataTableMessage<TransactionFilter> dataTableMessage)
        {
            var searchMessage = DataTableMessageConverter.ToPagedFilterDefinition(dataTableMessage);
            var pagedList = await _walletService.GetOwnedStocks(CurrentUserId, searchMessage);
            var model = BuildCurrentDataTableResponse(dataTableMessage, pagedList);
            return new JsonNetResult(model, false);
        }

        [HttpPost]
        public async Task<ActionResult> GetTodaySignalsTable(DataTableMessage<TransactionFilter> dataTableMessage)
        {
            var searchMessage = DataTableMessageConverter.ToPagedFilterDefinition(dataTableMessage);
            var pagedList = await _indicatorsService.GetSignals(searchMessage);
            var model = BuildSignalsDataTableResponse(dataTableMessage, pagedList);
            return new JsonNetResult(model, false);
        }
        
        [HttpPost]
        public async Task<ActionResult> GetAdvancersTable(DataTableMessage<TransactionFilter> dataTableMessage)
        {
            var searchMessage = DataTableMessageConverter.ToPagedFilterDefinition(dataTableMessage);
            var pagedList = await _priceService.GetAdvancers(searchMessage);
            var model = BuildAdvancersDataTableResponse(dataTableMessage, pagedList);
            return new JsonNetResult(model, false);
        }

        [HttpPost]
        public async Task<ActionResult> GetDeclinersTable(DataTableMessage<TransactionFilter> dataTableMessage)
        {
            var searchMessage = DataTableMessageConverter.ToPagedFilterDefinition(dataTableMessage);
            var pagedList = await _priceService.GetDecliners(searchMessage);
            var model = BuildAdvancersDataTableResponse(dataTableMessage, pagedList);
            return new JsonNetResult(model, false);
        }

        [HttpPost]
        public async Task<ActionResult> GetMostActiveTable(DataTableMessage<TransactionFilter> dataTableMessage)
        {
            var searchMessage = DataTableMessageConverter.ToPagedFilterDefinition(dataTableMessage);
            var pagedList = await _priceService.GetMostActive(searchMessage);
            var model = BuildAdvancersDataTableResponse(dataTableMessage, pagedList);
            return new JsonNetResult(model, false);
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private static DataTableResponse<OwnedCompanyStocksDto> BuildCurrentDataTableResponse(DataTableMessage<TransactionFilter> dataTableMessage, PagedList<OwnedCompanyStocksDto> pagedList)
        {
            var model = new DataTableResponse<OwnedCompanyStocksDto>
            {
                RecordsFiltered = pagedList.TotalCount,
                RecordsTotal = pagedList.TotalCount,
                Data = pagedList,
                Draw = dataTableMessage.Draw
            };
            return model;
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private static DataTableResponse<TodaySignal> BuildSignalsDataTableResponse(DataTableMessage<TransactionFilter> dataTableMessage, PagedList<TodaySignal> pagedList)
        {
            var model = new DataTableResponse<TodaySignal>
            {
                RecordsFiltered = pagedList.TotalCount,
                RecordsTotal = pagedList.TotalCount,
                Data = pagedList,
                Draw = dataTableMessage.Draw
            };
            return model;
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private static DataTableResponse<MostActivePriceDto> BuildAdvancersDataTableResponse(DataTableMessage<TransactionFilter> dataTableMessage, PagedList<MostActivePriceDto> pagedList)
        {
            var model = new DataTableResponse<MostActivePriceDto>
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
                CurrentSignalsCount = await _indicatorsService.GetSignalsCount()
            };
            return walletModel;
        }
    }
}