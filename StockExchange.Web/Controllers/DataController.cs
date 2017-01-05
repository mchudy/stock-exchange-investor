using System.Threading.Tasks;
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
        public async Task<ActionResult> Index()
        {
            var model = await GetPriceViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult GetPrices(DataTableMessage<PriceFilter> dataTableMessage)
        {
            var searchMessage = DataTableMessageConverter.ToPagedFilterDefinition(dataTableMessage);
            var pagedList = _priceService.GetPrices(searchMessage).Result;
            var model = BuildDataTableResponse(dataTableMessage, pagedList);
            return new JsonNetResult(model, false);
        }

        [HttpGet]
        public ActionResult GetFilterValues(DataTableSimpleMessage<PriceFilter> message, string fieldName)
        {
            var values = _priceService.GetFilterValues(DataTableMessageConverter.ToFilterDefinition(message), fieldName).Result;
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

        private async Task<PriceViewModel> GetPriceViewModel()
        {
            var model = new PriceViewModel
            {
                CompanyNames = await _companyService.GetCompanyNames(),
            };
            return model;
        }
    }
}