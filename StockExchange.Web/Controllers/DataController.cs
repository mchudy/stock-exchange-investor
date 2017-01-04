using StockExchange.Business.Extensions;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Helpers;
using StockExchange.Web.Models.DataTables;
using System.Web.Mvc;
using StockExchange.Business.Models.Price;
using StockExchange.Web.Helpers.Json;
using StockExchange.Web.Models.Price;

namespace StockExchange.Web.Controllers
{
    public sealed class DataController : BaseController
    {
        private readonly IPriceService _priceService;
        private readonly ICompanyService _companyService;

        public DataController(IPriceService priceService, ICompanyService companyService)
        {
            _priceService = priceService;
            _companyService = companyService;
        }

        [HttpGet]
        public ActionResult Index()
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