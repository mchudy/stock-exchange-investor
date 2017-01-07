using StockExchange.Business.Models.Company;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Paging;
using StockExchange.Business.Models.Transaction;
using StockExchange.Business.Models.Wallet;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Filters;
using StockExchange.Web.Helpers;
using StockExchange.Web.Helpers.Json;
using StockExchange.Web.Models.DataTables;
using StockExchange.Web.Models.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WalletViewModel = StockExchange.Web.Models.Wallet.WalletViewModel;

namespace StockExchange.Web.Controllers
{
    /// <summary>
    /// Controller for user wallet actions
    /// </summary>
    [Authorize]
    public class WalletController : BaseController
    {
        private readonly IWalletService _walletService;
        private readonly ITransactionsService _transactionsService;
        private readonly ICompanyService _companyService;
        private readonly IUserService _userService;

        /// <summary>
        /// Creates a new instance of <see cref="WalletController"/>
        /// </summary>
        /// <param name="transactionsService"></param>
        /// <param name="companyService"></param>
        /// <param name="walletService"></param>
        /// <param name="userService"></param>
        public WalletController(ITransactionsService transactionsService, ICompanyService companyService, IWalletService walletService, IUserService userService)
        {
            _transactionsService = transactionsService;
            _companyService = companyService;
            _walletService = walletService;
            _userService = userService;
        }

        /// <summary>
        /// Returns the index view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var companies = await _companyService.GetCompanies();
            var model = await GetTransactionViewModel(companies);
            return View(model);
        }

        /// <summary>
        /// Returns user budget info
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetBudget()
        {
            var ownedStocks = await _walletService.GetOwnedStocks(CurrentUserId);
            return new JsonNetResult(new BudgetInfoViewModel
            {
                FreeBudget = CurrentUser.Budget,
                AllStocksValue = ownedStocks.Sum(s => s.CurrentValue)
            });
        }

        /// <summary>
        /// Returns data for the transactions table
        /// </summary>
        /// <param name="dataTableMessage"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetTransactionsTable(DataTableMessage<TransactionFilter> dataTableMessage)
        {
            var searchMessage = DataTableMessageConverter.ToPagedFilterDefinition(dataTableMessage);
            var pagedList = await _transactionsService.GetTransactions(CurrentUserId, searchMessage);
            var model = BuildDataTableResponse(dataTableMessage, pagedList);
            return new JsonNetResult(model, false);
        }

        /// <summary>
        /// Returns data for the current transactions table
        /// </summary>
        /// <param name="dataTableMessage"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetCurrentTransactionsTable(DataTableMessage<TransactionFilter> dataTableMessage)
        {
            var searchMessage = DataTableMessageConverter.ToPagedFilterDefinition(dataTableMessage);
            var pagedList = await _walletService.GetOwnedStocks(CurrentUserId, searchMessage);
            var model = BuildCurrentDataTableResponse(dataTableMessage, pagedList);
            return new JsonNetResult(model, false);
        }

        /// <summary>
        /// Adds a new user transaction
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleJsonError]
        public async Task<ActionResult> AddTransaction(WalletViewModel model)
        {
            if (!ModelState.IsValid)
                return JsonErrorResult(ModelState);

            var dto = BuildUserTransactionDto(model.AddTransactionViewModel);
            await _transactionsService.AddTransaction(dto);
            return new JsonNetResult(new { dto.Id });
        }

        /// <summary>
        /// Returns the dialog for editing user budget
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditBudgetDialog()
        {
            var model = new UpdateBudgetViewModel { NewBudget = CurrentUser.Budget };
            return PartialView("_EditBudgetDialog", model);
        }

        /// <summary>
        /// Edits the user budget
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EditBudget(UpdateBudgetViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonErrorResult(ModelState);
            }
            await _userService.EditBudget(CurrentUserId, model.NewBudget);
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

        private async Task<WalletViewModel> GetTransactionViewModel(IList<CompanyDto> companies)
        {
            var ownedStocks = await _walletService.GetOwnedStocks(CurrentUserId);
            return new WalletViewModel
            {
                AddTransactionViewModel =
                    new AddTransactionViewModel { Companies = companies, Date = DateTime.Today },
                BudgetInfo = new BudgetInfoViewModel
                {
                    AllStocksValue = ownedStocks.Sum(s => s.CurrentValue),
                    FreeBudget = CurrentUser.Budget
                }
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