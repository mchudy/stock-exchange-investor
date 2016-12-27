using StockExchange.Business.Models.Indicators;
using StockExchange.Business.Models.Strategy;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Filters;
using StockExchange.Web.Helpers.Json;
using StockExchange.Web.Models.Indicator;
using StockExchange.Web.Models.Strategy;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
            var strategies = _strategyService.GetUserStrategies(CurrentUserId);
            return View(strategies);
        }

        [HttpGet]
        public ActionResult EditStrategy(int? id)
        {
            return View(GetViewModel());
        }

        [HttpPost]
        [HandleJsonError]
        public ActionResult EditStrategy(EditStrategyViewModel model)
        {
            //TODO: refactor
            if (!model.Indicators?.Any() ?? false)
            {
                Response.StatusCode = 400;
                return new JsonNetResult(new [] {new {message = "At least one indicator has to chosen"} });
            }
            var dto = BuildCreateStrategyDto(model);
            int id = _strategyService.CreateStrategy(dto);
            return new JsonNetResult(new {id});
        }


        private StrategyDto BuildCreateStrategyDto(EditStrategyViewModel model)
        {
            var dto = new StrategyDto
            {
                Name = model.Name,
                UserId = CurrentUserId,
                Indicators = model.Indicators.Select(i => new ParameterizedIndicator
                {
                    IndicatorType = i.Type,
                    Properties = i.Properties
                        .Where(p => !string.IsNullOrWhiteSpace(p.Name))
                        .Select(p => new IndicatorProperty
                        {
                            Name = p.Name,
                            Value = p.Value
                        }).ToList()
                }).ToList()
            };
            return dto;
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