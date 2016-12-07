using StockExchange.Business.Models;
using StockExchange.Business.Services;
using StockExchange.Web.Models;
using System;
using System.Web.Mvc;

namespace StockExchange.Web.Controllers
{
    [Authorize]
    public class StrategiesController : BaseController
    {
        private readonly IPriceService _priceService;
        private readonly IStrategyService _strategyService;
        private readonly IIndicatorsService _indicatorsService;

        public StrategiesController(IPriceService priceService, IStrategyService strategyService)
        {
            _priceService = priceService;
            _strategyService = strategyService;
        }

        // GET: Strategies
        public ActionResult Index()
        {
            var model = GetViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateStrategy(StrategyViewModel model)
        {
            var strategy = new StrategyDto
            {
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Companies = model.SelectedCompanyIds,
                UserId = CurrentUserId
            };
            _strategyService.CreateStrategy(strategy);
            return RedirectToAction("Index", "Wallet");
        }

        private StrategyViewModel GetViewModel()
        {
            var model = new StrategyViewModel
            {
                Companies = _priceService.GetAllCompanies(),
                StartDate = new DateTime(2006, 01, 01),
                EndDate = DateTime.Today
            };
            return model;
        }
    }
}