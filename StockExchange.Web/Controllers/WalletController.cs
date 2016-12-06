using StockExchange.Business.Models;
using StockExchange.Web.Models.Wallet;
using System.Collections.Generic;
using System.Web.Mvc;

namespace StockExchange.Web.Controllers
{
    [Authorize]
    public class WalletController : BaseController
    {

        [HttpGet]
        public ActionResult Index()
        {
            var walletModel = new WalletViewModel
            {
                Budget = 100,
                Transactions = new List<UserTransactionDto>()
            };
            return View(walletModel);
        }
    }
}