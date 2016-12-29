using StockExchange.Business.Extensions;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Transaction;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Filters;
using StockExchange.Web.Helpers;
using StockExchange.Web.Helpers.Json;
using StockExchange.Web.Models.DataTables;
using StockExchange.Web.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StockExchange.Business.Models.Wallet;
using StockExchange.Web.Models.Wallet;

namespace StockExchange.Web.Controllers
{
    [Authorize]
    public class TransactionsController : BaseController
    {
        private readonly IWalletService _walletService;
        private readonly ITransactionsService _transactionsService;
        private readonly ICompanyService _companyService;
        private readonly IUserService _userService;

        public TransactionsController(ITransactionsService transactionsService, ICompanyService companyService, IWalletService walletService, IUserService userService)
        {
            _transactionsService = transactionsService;
            _companyService = companyService;
            _walletService = walletService;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new AddTransactionViewModel
            {
                Companies = _companyService.GetAllCompanies()
            };
            var ownedStocks = _walletService.GetOwnedStocks(CurrentUserId);
            return View(new TransactionViewModel { AddTransactionViewModel = model, Transactions = new List<UserTransactionDto>(),
                FreeBudget = CurrentUser.Budget,
                AllStocksValue = ownedStocks.Sum(s => s.CurrentValue), CurrentTransactions = new List<OwnedCompanyStocksDto>()
            });
        }

        [HttpPost]
        public ActionResult GetTransactionsTable(DataTableMessage<TransactionFilter> dataTableMessage)
        {
            var searchMessage = DataTableMessageConverter.ToPagedFilterDefinition(dataTableMessage);
            var pagedList = _transactionsService.GetUserTransactions(CurrentUserId, searchMessage);
            var model = BuildDataTableResponse(dataTableMessage, pagedList);
            return new JsonNetResult(model, false);
        }

        [HttpPost]
        public ActionResult GetCurrentTransactionsTable(DataTableMessage<TransactionFilter> dataTableMessage)
        {
            var searchMessage = DataTableMessageConverter.ToPagedFilterDefinition(dataTableMessage);
            var pagedList = _walletService.GetOwnedStocks(CurrentUserId, searchMessage);
            var model = BuildCurrentDataTableResponse(dataTableMessage, pagedList);
            return new JsonNetResult(model, false);
        }

        [HttpPost]
        [HandleJsonError]
        public ActionResult AddTransaction(TransactionViewModel model)
        {
            if (!ModelState.IsValid)
                return JsonErrorResult(ModelState);

            var dto = BuildUserTransactionDto(model.AddTransactionViewModel);
            _transactionsService.AddUserTransaction(dto);
            return new JsonNetResult(new { dto.Id });
        }

        [HttpGet]
        public ActionResult EditBudgetDialog()
        {
            var model = new UpdateBudgetViewModel { NewBudget = CurrentUser.Budget };
            return PartialView("_EditBudgetDialog", model);
        }

        [HttpPost]
        public ActionResult EditBudget(UpdateBudgetViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditBudgetDialog");
            }
            _userService.EditBudget(CurrentUserId, model.NewBudget);
            return RedirectToAction("Index");
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private static DataTableResponse<UserTransactionDto> BuildDataTableResponse(DataTableMessage<TransactionFilter> dataTableMessage, PagedList<UserTransactionDto> pagedList)
        {
            var model = new DataTableResponse<UserTransactionDto>
            {
                RecordsFiltered = pagedList.TotalCount,
                RecordsTotal = pagedList.TotalCount,
                Data = pagedList,
                Draw = dataTableMessage.Draw
            };
            return model;
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

        private UserTransactionDto BuildUserTransactionDto(AddTransactionViewModel model)
        {
            return new UserTransactionDto
            {
                Date = DateTime.Now,
                CompanyId = model.SelectedCompanyId,
                Price = model.Price,
                Quantity = model.TransactionType == TransactionActionType.Buy ? model.Quantity : -model.Quantity,
                UserId = CurrentUserId
            };
        }
    }
}