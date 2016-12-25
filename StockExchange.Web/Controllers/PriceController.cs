using StockExchange.Business.Extensions;
using StockExchange.Business.Models;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Helpers;
using StockExchange.Web.Models;
using StockExchange.Web.Models.DataTables;
using System.Web.Mvc;

namespace StockExchange.Web.Controllers
{
    public sealed class PriceController : BaseController
    {
        private readonly IPriceService _priceService;
        private readonly ICompanyService _companyService;

        public PriceController(IPriceService priceService, ICompanyService companyService)
        {
            _priceService = priceService;
            _companyService = companyService;
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
            var model = BuildDataTableResponse(dataTableMessage, pagedList);
            return new JsonNetResult(model, false);
        }

        [HttpGet]
        public ActionResult GetFilterValues(DataTableSimpleMessage<PriceFilter> message, string fieldName)
        {
            var values = _priceService.GetValues(DataTableMessageConverter.ToFilterDefinition(message), fieldName);
            return new JsonNetResult(values, typeof(PriceDto), fieldName);
        }

        private static DataTableResponse<PriceDto> BuildDataTableResponse(DataTableMessage<PriceFilter> dataTableMessage, PagedList<PriceDto> pagedList)
        {
            var model = new DataTableResponse<PriceDto>
            {
                RecordsFiltered = pagedList.TotalCount,
                RecordsTotal = pagedList.TotalCount,
                Data = pagedList,
                Draw = dataTableMessage.Draw
            };
            return model;
        }

        private PriceViewModel GetPriceViewModel()
        {
            var model = new PriceViewModel
            {
                CompanyNames = _companyService.GetCompanyNames(),
            };
            return model;
        }
    }
}