using System.Web.Mvc;

namespace StockExchange.Web.Controllers
{
    public class ChartsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}