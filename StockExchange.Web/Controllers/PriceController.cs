using StockExchange.Business.Business;
using StockExchange.Business.Models;
using StockExchange.Web.Helpers;
using StockExchange.Web.Models;
using StockExchange.Web.Models.DataTables;
using System.Web.Mvc;

namespace StockExchange.Web.Controllers
{
    public sealed class PriceController : BaseController
    {
        private readonly IPriceManager _priceManager;

        public PriceController(IPriceManager priceManager)
        {
            _priceManager = priceManager;
        }

        [HttpGet]
        public ActionResult Price()
        {
            var model = GetPriceViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult GetPrices(DataTableMessage<PriceFilter> dataTableMessage)
        {
            var searchMessage = DataTableMessageConverter.ToPagedFilterDefinition(dataTableMessage);
            var pagedList = _priceManager.Get(searchMessage);
            var model = new DataTableResponse<PriceDto>
            {
                RecordsFiltered = pagedList.TotalCount,
                RecordsTotal = pagedList.TotalCount,
                Data = pagedList,
                Draw = dataTableMessage.Draw
            };
            return new JsonNetResult(model);
        }

        [HttpGet]
        public ActionResult GetFilterValues(DataTableSimpleMessage<PriceFilter> message, string fieldName)
        {
            return new JsonNetResult(_priceManager.GetValues(DataTableMessageConverter.ToFilterDefinition(message), fieldName), typeof(PriceDto), fieldName);
        }

        private PriceViewModel GetPriceViewModel()
        {
            var model = new PriceViewModel
            {
                CompanyNames = _priceManager.GetCompanyNames(),
            };
            return model;
        }
    }
}