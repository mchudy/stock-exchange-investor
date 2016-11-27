using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using StockExchange.Business.Services;
using StockExchange.Web.Models.Wallet;

namespace StockExchange.Web.Controllers
{
    [Authorize]
    public class WalletController : BaseController
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var wallet = _walletService.GetUserWallet(CurrentUserId);
            if (wallet == null)
                return RedirectToAction("CreateWallet");
            var walletModel = new WalletViewModel()
            {
                Budget = wallet.Budget,
                Transactions = wallet.Transactions
            };
            return View(walletModel);
        }

        [HttpGet]
        public ActionResult CreateWallet()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateWallet(CreateWalletViewModel model)
        {
            if(_walletService.CreateWallet(CurrentUserId, model.Budget))
                return RedirectToAction("Index", "Wallet");
            return new EmptyResult();
        }
    }
}