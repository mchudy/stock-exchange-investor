using System;
using System.Web.Mvc;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Models.Simulation;

namespace StockExchange.Web.Controllers
{
    [Authorize]
    public class SimulationsController : BaseController
    {
        private readonly IStrategyService _strategyService;
        private readonly ICompanyService _companyService;

        public SimulationsController(IStrategyService strategyService, ICompanyService companyService)
        {
            _strategyService = strategyService;
            _companyService = companyService;
        }

        public ActionResult Index()
        {
            var model = GetViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult RunSimulation(SimulationViewModel model)
        {
            return RedirectToAction("Results");
        }

        [HttpGet]
        public ActionResult Results()
        {
            return View();
        }

        private SimulationViewModel GetViewModel()
        {
            var model = new SimulationViewModel
            {
                Companies = _companyService.GetAllCompanies(),
                StartDate = new DateTime(2006, 01, 01),
                EndDate = DateTime.Today, 
                Strategies = _strategyService.GetUserStrategies(CurrentUserId)
            };
            return model;
        }
    }
}