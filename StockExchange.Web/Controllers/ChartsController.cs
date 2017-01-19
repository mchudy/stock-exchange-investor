using StockExchange.Business.Indicators.Common;
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
    /// <summary>
    /// Controller for charts actions
    /// </summary>
    public class ChartsController : BaseController
    {
        private readonly IPriceService _priceService;
        private readonly IIndicatorsService _indicatorsService;
        private readonly ICompanyService _companyService;

        /// <summary>
        /// Creates a new instance of <see cref="ChartsController"/>
        /// </summary>
        /// <param name="priceService"></param>
        /// <param name="indicatorsService"></param>
        /// <param name="companyService"></param>
        public ChartsController(IPriceService priceService, IIndicatorsService indicatorsService, ICompanyService companyService)
        {
            _priceService = priceService;
            _indicatorsService = indicatorsService;
            _companyService = companyService;
        }

        /// <summary>
        /// Returns the index view
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var model = await BuildChartIndexModel();
            return View(model);
        }

        /// <summary>
        /// Returns line chart stock JSON data
        /// </summary>
        /// <param name="companyIds"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns candlestick chart stock JSON data
        /// </summary>
        /// <param name="companyIds"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetCandlestickChartData(IList<int> companyIds)
        {
            var companyPrices = await _priceService.GetPrices(companyIds);
            var model = BuildCandlestickChartModel(companyPrices);
            return new JsonNetResult(model);
        }

        /// <summary>
        /// Returns indicator values JSON data
        /// </summary>
        /// <param name="companyIds">Companies to include</param>
        /// <param name="type">Indicator type to compute</param>
        /// <param name="properties">Indicator properties</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetIndicatorValues(IList<int> companyIds, IndicatorType type, IList<IndicatorProperty> properties)
        {
            var values = await _indicatorsService.GetIndicatorValues(type, companyIds, properties);
            var model = BuildIndicatorChartModel(values);
            return new JsonNetResult(model);
        }

        private async Task<ChartsIndexModel> BuildChartIndexModel()
        {
            var companies = await _companyService.GetCompanies();
            var groups = await _companyService.GetCompanyGroups();

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
                    }).ToList(),
                CompanyGroups = groups
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