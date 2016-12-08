using StockExchange.Business.Models;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Models.Wallet;
using System.Collections.Generic;
using System.Web.Mvc;

namespace StockExchange.Web.Controllers
{
    [Authorize]
    public class WalletController : BaseController
    {
        private readonly IUserService _userService;

        public WalletController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var walletModel = new WalletViewModel
            {
                Budget = CurrentUser.Budget,
                Transactions = new List<UserTransactionDto>()
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