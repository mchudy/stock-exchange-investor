using System;
using System.Linq;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Models;
using System.Web.Mvc;
using StockExchange.Business.Indicators;
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
        public ActionResult CreateStrategy(StrategyViewModel model)
        {
            //var strategy = new StrategyDto
            //{
            //    StartDate = model.StartDate,
            //    EndDate = model.EndDate,
            //    Companies = model.SelectedCompanyIds,
            //    UserId = CurrentUserId
            //};
            //_strategyService.CreateStrategy(strategy);
            
            return RedirectToAction("Index", "Wallet");
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
            return model;
        }
    }
}