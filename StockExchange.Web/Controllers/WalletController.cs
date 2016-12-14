using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Models.Wallet;
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
            var walletModel = new WalletViewModel
            {
                FreeBudget = CurrentUser.Budget,
                AllStocksValue = ownedStocks.Sum(s => s.CurrentValue),
                AllTransactionsCount = _transactionsService.GetUserTransactionsCount(CurrentUserId),
                OwnedCompanyStocks = ownedStocks
            };
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
                return PartialView("_EditBudgetDialog");
            }
            _userService.EditBudget(CurrentUserId, model.NewBudget);
            return RedirectToAction("Index");
        }
    }
}