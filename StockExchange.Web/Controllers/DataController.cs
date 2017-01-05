using StockExchange.Business.Extensions;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Price;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Helpers;
using StockExchange.Web.Helpers.Json;
using StockExchange.Web.Models.DataTables;
using StockExchange.Web.Models.Price;
using System.Threading.Tasks;
using System.Web.Mvc;

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
        public async Task<ActionResult> GetPrices(DataTableMessage<PriceFilter> dataTableMessage)
        {
            var searchMessage = DataTableMessageConverter.ToPagedFilterDefinition(dataTableMessage);
            var pagedList = await _priceService.GetPrices(searchMessage);
            var model = BuildDataTableResponse(dataTableMessage, pagedList);
            return new JsonNetResult(model, false);
        }

        [HttpGet]
        public async Task<ActionResult> GetFilterValues(DataTableSimpleMessage<PriceFilter> message, string fieldName)
        {
            var values = await _priceService.GetFilterValues(DataTableMessageConverter.ToFilterDefinition(message), fieldName);
            return new JsonNetResult(values, typeof(PriceDto), fieldName);
        }

        private static DataTableResponse<PriceDto> BuildDataTableResponse(DataTableMessage<PriceFilter> dataTableMessage, PagedList<PriceDto> pagedList)
        {
            return new DataTableResponse<PriceDto>
            {
                RecordsFiltered = pagedList.TotalCount,
                RecordsTotal = pagedList.TotalCount,
                Data = pagedList,
                Draw = dataTableMessage.Draw
            };
        }

        private async Task<PriceViewModel> GetPriceViewModel()
        {
            return new PriceViewModel
            {
                CompanyNames = await _companyService.GetCompanyNames(),
            };
        }
    }
}