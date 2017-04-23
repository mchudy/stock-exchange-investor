using System.Web.Mvc;

namespace StockExchange.Web.Controllers
{
    /// <summary>
    /// Donate Controller
    /// </summary>
    public class DonateController : BaseController
    {
        // GET: Donate
        /// <summary>
        /// Donate View
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}