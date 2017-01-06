using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Company;
using StockExchange.Business.Models.Indicators;
using StockExchange.Business.Models.Price;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Helpers;
using StockExchange.Web.Helpers.Json;
using StockExchange.Web.Models.Charts;
using StockExchange.Web.Models.Indicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StockExchange.Web.Controllers
{
    public class ChartsController : BaseController
    {
        private readonly IPriceService _priceService;
        private readonly IIndicatorsService _indicatorsService;
        private readonly ICompanyService _companyService;

        public ChartsController(IPriceService priceService, IIndicatorsService indicatorsService, ICompanyService companyService)
        {
            _priceService = priceService;
            _indicatorsService = indicatorsService;
            _companyService = companyService;
        }

        public async Task<ActionResult> Index()
        {
            var companies = await _companyService.GetCompanies();
            var model = BuildChartIndexModel(companies);
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> GetLineChartData(IList<int> companyIds)
        {
            var companyPrices = await _priceService.GetPrices(companyIds);
            var model = companyPrices.Select(cp => new LineChartModel
            {
                CompanyId = cp.Company.Id,
                Name = cp.Company.Code,
                Data = cp.Prices.Select(p => new[] { p.Date.ToJavaScriptTimeStamp(), p.ClosePrice }).ToList()
            });
            return new JsonNetResult(model);
        }

        [HttpGet]
        public async Task<ActionResult> GetCandlestickChartData(IList<int> companyIds)
        {
            var companyPrices = await _priceService.GetPrices(companyIds);
            var model = BuildCandlestickChartModel(companyPrices);
            return new JsonNetResult(model);
        }

        [HttpGet]
        public async Task<ActionResult> GetIndicatorValues(IList<int> companyIds, IndicatorType type, IList<IndicatorProperty> properties)
        {
            var values = await _indicatorsService.GetIndicatorValues(type, companyIds, properties);
            var model = BuildIndicatorChartModel(values);
            return new JsonNetResult(model);
        }

        private ChartsIndexModel BuildChartIndexModel(IList<CompanyDto> companies)
        {
            return new ChartsIndexModel
            {
                Companies = companies,
                Indicators = _indicatorsService.GetAllIndicators()
                    .Select(i => new EditIndicatorViewModel
                    {
                        Name = i.IndicatorName,
                        Type = i.IndicatorType,
                        Properties = _indicatorsService.GetPropertiesForIndicator(i.IndicatorType)
                            .Select(p => new IndicatorPropertyViewModel {Name = p.Name, Value = p.Value}).ToList()
                    }).ToList()
            };
        }

        private static IEnumerable<LineChartModel> BuildIndicatorChartModel(IList<CompanyIndicatorValues> values)
        {
            return values.Select(cv => new LineChartModel
            {
                CompanyId = cv.Company.Id,
                Name = cv.Company.Code,
                Data = ConvertIndicatorValuesToData(cv.IndicatorValues)
            });
        }

        private static IList<decimal[]> ConvertIndicatorValuesToData(IList<IndicatorValue> values)
        {
            //TODO: needs refactoring, the inheritance of IndicatorValue is a bit troublesome
            if (values.FirstOrDefault() is DoubleLineIndicatorValue)
            {
                return values.Cast<DoubleLineIndicatorValue>().Select(v => new[]
                {
                    v.Date.ToJavaScriptTimeStamp(),
                    decimal.Round(v.Value, 3, MidpointRounding.AwayFromZero),
                    decimal.Round(v.SecondLineValue, 3, MidpointRounding.AwayFromZero)
            }).ToList();
            }
            return values.Select(v => new[]
            {
                v.Date.ToJavaScriptTimeStamp(),
                decimal.Round(v.Value, 3, MidpointRounding.AwayFromZero)
            }).ToList();
        }

        private static IEnumerable<LineChartModel> BuildCandlestickChartModel(IList<CompanyPricesDto> companyPrices)
        {
            return companyPrices.Select(cp => new LineChartModel
            {
                CompanyId = cp.Company.Id,
                Name = cp.Company.Code,
                Data = cp.Prices.Select(p => new[]
                {
                    p.Date.ToJavaScriptTimeStamp(),
                    p.OpenPrice,
                    p.HighPrice,
                    p.LowPrice,
                    p.ClosePrice
                }).ToList()
            });
        }
    }
}