using StockExchange.Business.Models.Indicators;
using StockExchange.Business.Models.Strategy;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Filters;
using StockExchange.Web.Helpers.Json;
using StockExchange.Web.Helpers.ToastNotifications;
using StockExchange.Web.Models.Indicator;
using StockExchange.Web.Models.Strategy;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<ActionResult> Index()
        {
            var strategies = await _strategyService.GetStrategies(CurrentUserId);
            return View(strategies);
        }

        [HttpGet]
        public async Task<ActionResult> EditStrategy(int? id)
        {
            var model = await GetViewModel(id);
            return View(model);
        }

        [HttpPost]
        [HandleJsonError]
        public async Task<ActionResult> EditStrategy(EditStrategyViewModel model)
        {
            if (!model.Indicators?.Any() ?? false)
            {
                return JsonErrorResult("Strategy must have at least one indicator");
            }
            var dto = BuildCreateStrategyDto(model);
            int? id = model.Id;
            if (model.Id.HasValue)
            {
                await _strategyService.UpdateStrategy(dto);
            }
            else
            {
                id = await _strategyService.CreateStrategy(dto);
            }
            ShowNotification("", "Strategy has been saved", ToastType.Success);
            return new JsonNetResult(new {id, redirectUrl = Url.Action("Index")});
        }

        [HttpPost]
        [HandleJsonError]
        public ActionResult DeleteStrategy(int id)
        {
            _strategyService.DeleteStrategy(id, CurrentUserId);
            ShowNotification("Success", "Strategy has been deleted", ToastType.Success);
            return new JsonNetResult(true);
        }

        private StrategyDto BuildCreateStrategyDto(EditStrategyViewModel model)
        {
            var dto = new StrategyDto
            {
                Name = model.Name,
                UserId = CurrentUserId,
                Id = model.Id ?? 0,
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

        private async Task<EditStrategyViewModel> GetViewModel(int? id)
        {
            var model = new EditStrategyViewModel
            {
                Indicators = _indicatorsService.GetAllIndicators().Select(dto=>new EditIndicatorViewModel()
                {
                    Name = dto.IndicatorName,
                    Type = dto.IndicatorType
                }).ToList()
            };
            foreach (var indicator in model.Indicators)
            {
                var properties = _indicatorsService.GetPropertiesForIndicator(indicator.Type);
                indicator.Properties = properties.Select(p => new IndicatorPropertyViewModel {Name = p.Name, Value = p.Value}).ToList();
            }
            if (id.HasValue)
            {
                model.Id = id;
                await FillIndicatorValues(id, model);
            }
            return model;
        }

        [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
        private async Task FillIndicatorValues(int? id, EditStrategyViewModel model)
        {
            var strategy = await _strategyService.GetStrategy(CurrentUserId, id.Value);
            model.Name = strategy.Name;
            foreach (var parameterizedIndicator in strategy.Indicators.Where(t => t.IndicatorType.HasValue))
            {
                var indicatorModel =
                    model.Indicators.FirstOrDefault(m => m.Type == parameterizedIndicator.IndicatorType.Value);
                if (indicatorModel == null)
                    continue;

                indicatorModel.IsSelected = true;
                foreach (var property in parameterizedIndicator.Properties)
                {
                    var propertyModel = indicatorModel.Properties.FirstOrDefault(p => p.Name == property.Name);
                    if(propertyModel == null)
                        continue;

                    propertyModel.Value = property.Value;
                }
            }
        }
    }
}