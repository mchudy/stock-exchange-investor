using System.Web.Mvc;

namespace StockExchange.Web.Controllers
{
    public class ChartsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}