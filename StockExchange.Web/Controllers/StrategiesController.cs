using StockExchange.Business.Models.Indicators;
using StockExchange.Business.Models.Strategy;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StockExchange.Web.Helpers.Json;
using StockExchange.Web.Models.Indicator;
using StockExchange.Web.Models.Strategy;

namespace StockExchange.Web.Controllers
{
    [Authorize]
    public class StrategiesController : BaseController
    {
        private readonly IStrategyService _strategyService;
        private readonly IIndicatorsService _indicatorsService;

        public StrategiesController(IStrategyService strategyService, IIndicatorsService indicatorsService)
        {
            _strategyService = strategyService;
            _indicatorsService = indicatorsService;
        }

        public ActionResult Index()
        {
            var model = GetViewModel();
            return View(model);
        }

        [HttpPost]
        [HandleJsonError]
        public ActionResult CreateStrategy(IList<IndicatorMessage> indicators)
        {
            //TODO: refactor
            if (!indicators?.Any() ?? false)
            {
                Response.StatusCode = 400;
                return new JsonNetResult(new [] {new {message = "At least one indicator has to chosen"} });
            }
            var dto = BuildCreateStrategyDto(indicators);
            int id = _strategyService.CreateStrategy(dto);
            return new JsonNetResult(new {id});
        }

        [HttpGet]
        public ActionResult StrategiesTable()
        {
            var strategies = _strategyService.GetUserStrategies(CurrentUserId);
            return PartialView("_StrategiesTable", strategies);
        }

        private StrategyDto BuildCreateStrategyDto(IList<IndicatorMessage> indicators)
        {
            var dto = new StrategyDto
            {
                Name = indicators[0]?.Indicator,
                UserId = CurrentUserId,
                Indicators = ConvertPropertiesToIndicators(indicators.Skip(1))
            };
            return dto;
        }

        private IList<ParameterizedIndicator> ConvertPropertiesToIndicators(IEnumerable<IndicatorMessage> indicators)
        {
            var indicatorsDictionary = new Dictionary<string, IList<IndicatorProperty>>();
            foreach (var indicatorMessage in indicators)
            {
                if (!indicatorsDictionary.ContainsKey(indicatorMessage.Indicator))
                    indicatorsDictionary.Add(indicatorMessage.Indicator, new List<IndicatorProperty>());
                int value;
                if (!int.TryParse(indicatorMessage.Value, out value))
                    value = 0;
                indicatorsDictionary[indicatorMessage.Indicator].Add(new IndicatorProperty
                {
                    Name = indicatorMessage.Property,
                    Value = value
                });
            }
            return indicatorsDictionary.Select(item => new ParameterizedIndicator
            {
                IndicatorType = _indicatorsService.GetTypeFromName(item.Key),
                Properties = item.Value.Where(v => !string.IsNullOrWhiteSpace(v.Name)).ToList()
            }).ToList();
        } 

        private StrategyViewModel GetViewModel()
        {
            var model = new StrategyViewModel
            {
                Indicators = _indicatorsService.GetIndicatorsForStrategy().Select(dto=>new IndicatorViewModel()
                {
                    Name = dto.IndicatorName,
                    Type = dto.IndicatorType
                }).ToList()
            };
            var dictionary = new Dictionary<IndicatorProperty, IndicatorViewModel>();
            foreach (var indicator in model.Indicators)
            {
                var properties = _indicatorsService.GetPropertiesForIndicator(indicator.Type);
                foreach (var property in properties)
                {
                    dictionary.Add(property, indicator);
                }
            }
            model.Properties = dictionary;
            return model;
        }
    }
}