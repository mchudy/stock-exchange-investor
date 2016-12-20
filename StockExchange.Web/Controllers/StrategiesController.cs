using System;
using System.Collections.Generic;
using System.Linq;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Models;
using System.Web.Mvc;
using StockExchange.Business.Indicators;
using StockExchange.Business.Models;
using StockExchange.Business.Models.Indicators;
using StockExchange.Business.Models.Strategy;
using StockExchange.Common.Extensions;

namespace StockExchange.Web.Controllers
{
    [Authorize]
    public class StrategiesController : BaseController
    {
        private readonly IPriceService _priceService;
        private readonly IStrategyService _strategyService;
        private readonly IIndicatorsService _indicatorsService;

        public StrategiesController(IPriceService priceService, IStrategyService strategyService, IIndicatorsService indicatorsService)
        {
            _priceService = priceService;
            _strategyService = strategyService;
            _indicatorsService = indicatorsService;
        }

        public ActionResult Index()
        {
            var model = GetViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateStrategy(IList<IndicatorMessage> indicators)
        {
            var dto = new CreateStrategyDto
            {
                Name = indicators[0].Indicator,
                UserId = CurrentUserId,
                Indicators = ConvertPropertiesToIndicators(indicators.Skip(1))
            };
            _strategyService.CreateStrategy(dto);
            return RedirectToAction("Index", "Wallet");
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
                indicatorsDictionary[indicatorMessage.Indicator].Add(new IndicatorProperty()
                {
                    Name = indicatorMessage.Property,
                    Value = value
                });
            }
            return indicatorsDictionary.Select(item => new ParameterizedIndicator()
            {
                IndicatorType = _indicatorsService.GetTypeFromName(item.Key),
                Properties = item.Value
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