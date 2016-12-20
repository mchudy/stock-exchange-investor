using System;
using System.Collections.Generic;
using System.Linq;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Models;
using System.Web.Mvc;
using StockExchange.Business.Indicators;
using StockExchange.Business.Models;
using StockExchange.Business.Models.Indicators;
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
            var dto = new StrategyDto
            {
                Id = -1,
                Name = indicators[0].Indicator,
                UserId = 1
            };
            var dictionary = indicators.Skip(1).ToDictionary(item => new IndicatorProperty
            {
                Name = item.Property, Value = int.Parse(item.Value)
            }, item => item.Indicator);
            dto.Indicators = dictionary;
            _strategyService.CreateStrategy(dto);
            var model = GetViewModel();
            return View("Index", model);
        }

        private StrategyViewModel GetViewModel()
        {
            var model = new StrategyViewModel
            {
                Indicators = Enum.GetValues(typeof(IndicatorType)).Cast<IndicatorType>()
                    .Select(i => new IndicatorViewModel
                    {
                        Name = i.GetEnumDescription(),
                        Type = i
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