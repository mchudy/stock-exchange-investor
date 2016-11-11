using StockExchange.Business.Business;
using StockExchange.Business.Models;
using StockExchange.Web.Helpers;
using StockExchange.Web.Models.Charts;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StockExchange.Web.Controllers
{
    public class ChartsController : BaseController
    {
        private readonly IPriceManager _priceManager;

        public ChartsController(IPriceManager priceManager)
        {
            _priceManager = priceManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetLineChartData(IList<int> companyIds)
        {
            IList<CompanyPricesDto> companyPrices = _priceManager.GetPricesForCompanies(companyIds);
            var model = companyPrices.Select(cp => new LineChartModel
            {
                CompanyId = cp.Company.Id,
                Name = cp.Company.Code,
                Data = cp.Prices.Select(p => new[] { p.Date.ToJavaScriptTimeStamp(), p.ClosePrice }).ToList()
            });
            return new JsonNetResult(model);
        }

        [HttpGet]
        public ActionResult GetCandlestickChartData(IList<int> companyIds)
        {
            IList<CompanyPricesDto> companyPrices = _priceManager.GetPricesForCompanies(companyIds);
            var model = companyPrices.Select(cp => new LineChartModel
            {
                CompanyId = cp.Company.Id,
                Name = cp.Company.Code,
                Data = cp.Prices.Select(p => new[] { p.Date.ToJavaScriptTimeStamp(), p.OpenPrice, p.HighPrice, p.LowPrice, p.ClosePrice }).ToList()
            });
            return new JsonNetResult(model);
        }
    }
}