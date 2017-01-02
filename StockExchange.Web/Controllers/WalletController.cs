using StockExchange.Business.Extensions;
using StockExchange.Business.Models.Company;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Transaction;
using StockExchange.Business.Models.Wallet;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Filters;
using StockExchange.Web.Helpers;
using StockExchange.Web.Helpers.Json;
using StockExchange.Web.Models.DataTables;
using StockExchange.Web.Models.Transactions;
using StockExchange.Web.Models.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StockExchange.Web.Controllers
{
    [Authorize]
    public class WalletController : BaseController
    {
        private readonly IWalletService _walletService;
        private readonly ITransactionsService _transactionsService;
        private readonly ICompanyService _companyService;
        private readonly IUserService _userService;

        public WalletController(ITransactionsService transactionsService, ICompanyService companyService, IWalletService walletService, IUserService userService)
        {
            _transactionsService = transactionsService;
            _companyService = companyService;
            _walletService = walletService;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var companies = _companyService.GetAllCompanies();
            var model = GetTransactionViewModel(companies);
            return View(model);
        }

        [HttpGet]
        public ActionResult GetBudget()
        {
            var ownedStocks = _walletService.GetOwnedStocks(CurrentUserId);
            return new JsonNetResult(new BudgetInfoViewModel
            {
                FreeBudget = CurrentUser.Budget,
                AllStocksValue = ownedStocks.Sum(s => s.CurrentValue)
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
                return JsonErrorResult(ModelState);
            }
            _userService.EditBudget(CurrentUserId, model.NewBudget);
            return new JsonNetResult(new { UserId = CurrentUserId, model.NewBudget });
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

        private TransactionViewModel GetTransactionViewModel(IList<CompanyDto> companies)
        {
            return new TransactionViewModel
            {
                AddTransactionViewModel =
                    new AddTransactionViewModel { Companies = companies, Date = DateTime.Today },
                BudgetInfo = new BudgetInfoViewModel()
            };
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
                Date = model.Date,
                CompanyId = model.SelectedCompanyId,
                Price = model.Price,
                Quantity = model.TransactionType == TransactionActionType.Buy ? model.Quantity : -model.Quantity,
                UserId = CurrentUserId
            };
        }
    }
}