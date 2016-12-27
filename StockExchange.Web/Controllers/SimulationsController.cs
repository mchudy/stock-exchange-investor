using StockExchange.Business.Models.Simulations;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Models.Simulation;
using System;
using System.Web.Mvc;

namespace StockExchange.Web.Controllers
{
    [Authorize]
    public class SimulationsController : BaseController
    {
        private readonly IStrategyService _strategyService;
        private readonly ICompanyService _companyService;
        private readonly ISimulationService _simulationService;

        public SimulationsController(IStrategyService strategyService, ICompanyService companyService, ISimulationService simulationService)
        {
            _strategyService = strategyService;
            _companyService = companyService;
            _simulationService = simulationService;
        }

        [HttpGet]
        public ActionResult RunSimulation()
        {
            var model = GetViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult RunSimulation(SimulationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Strategies = _strategyService.GetUserStrategies(CurrentUserId);
                model.Companies = _companyService.GetAllCompanies();
                return View(model);
            }

            var ret = _simulationService.RunSimulation(ConvertViewModelToDto(model));
            return View("Results", ret);
        }

        [HttpGet]
        public ActionResult Results()
        {
            return View();
        }

        private SimulationDto ConvertViewModelToDto(SimulationViewModel viewModel)
        {
            return new SimulationDto
            {
                StartDate = viewModel.StartDate,
                EndDate = viewModel.EndDate,
                SelectedStrategyId = viewModel.SelectedStrategyId,
                SelectedCompanyIds = viewModel.SelectedCompanyIds,
                UserId = CurrentUserId,
                Budget = viewModel.Budget
            };
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