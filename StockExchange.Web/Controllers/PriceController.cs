using StockExchange.Business.Models;
using StockExchange.Web.Helpers;
using StockExchange.Web.Models;
using StockExchange.Web.Models.DataTables;
using System.Web.Mvc;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Services;

namespace StockExchange.Web.Controllers
{
    public sealed class PriceController : BaseController
    {
        private readonly IPriceService _priceService;

        public PriceController(IPriceService priceService)
        {
            _priceService = priceService;
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
            var pagedList = _priceService.Get(searchMessage);
            var model = new DataTableResponse<PriceDto>
            {
                RecordsFiltered = pagedList.TotalCount,
                RecordsTotal = pagedList.TotalCount,
                Data = pagedList,
                Draw = dataTableMessage.Draw
            };
            return new JsonNetResult(model, false);
        }

        [HttpGet]
        public ActionResult GetFilterValues(DataTableSimpleMessage<PriceFilter> message, string fieldName)
        {
            var values = _priceService.GetValues(DataTableMessageConverter.ToFilterDefinition(message), fieldName);
            return new JsonNetResult(values, typeof(PriceDto), fieldName);
        }

        private PriceViewModel GetPriceViewModel()
        {
            var model = new PriceViewModel
            {
                CompanyNames = _priceService.GetCompanyNames(),
            };
            return model;
        }
    }
}