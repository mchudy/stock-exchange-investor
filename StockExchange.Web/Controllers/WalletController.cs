using StockExchange.Business.Models.Wallet;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Helpers.Json;
using StockExchange.Web.Models.Charts;
using StockExchange.Web.Models.Wallet;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StockExchange.Web.Controllers
{
    [Authorize]
    public class WalletController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ITransactionsService _transactionsService;
        private readonly IWalletService _walletService;

        public WalletController(IUserService userService, ITransactionsService transactionsService, IWalletService walletService)
        {
            _userService = userService;
            _transactionsService = transactionsService;
            _walletService = walletService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var ownedStocks = _walletService.GetOwnedStocks(CurrentUserId);
            var walletModel = BuildWalletViewModel(ownedStocks);
            return View(walletModel);
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

        private WalletViewModel BuildWalletViewModel(IList<OwnedCompanyStocksDto> ownedStocks)
        {
            var walletModel = new WalletViewModel
            {
                BudgetInfo = new BudgetInfoViewModel
                {
                    AllStocksValue = ownedStocks.Sum(s => s.CurrentValue),
                    FreeBudget = CurrentUser.Budget
                },
                AllTransactionsCount = _transactionsService.GetUserTransactionsCount(CurrentUserId),
                OwnedCompanyStocks = ownedStocks,
                StocksByValue = new PieChartModel
                {
                    Title = "Owned stocks by value (PLN)",
                    Data = ownedStocks.Select(g => new PieChartEntry { Name = g.CompanyName, Value = g.CurrentValue }).ToList()
                }
            };
            return walletModel;
        }
    }
}