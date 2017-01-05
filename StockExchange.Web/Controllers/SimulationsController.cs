using StockExchange.Business.Models.Simulations;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Models.Simulation;
using System;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> RunSimulation()
        {
            var model = await GetViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> RunSimulation(SimulationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Strategies = await _strategyService.GetStrategies(CurrentUserId);
                model.Companies = await _companyService.GetCompanies();
                return View(model);
            }

            var ret = await _simulationService.RunSimulation(await ConvertViewModelToDto(model));
            var ids = ret.CurrentCompanyQuantity.Keys.ToList();
            ids.AddRange(ret.TransactionsLog.Select(item => item.CompanyId));
            var companies = await _companyService.GetCompanies(ids);
            return View("Results", new SimulationResultViewModel
            {
                CurrentCompanyQuantity = ret.CurrentCompanyQuantity.ToDictionary(item => companies.FirstOrDefault(x => x.Id == item.Key), item => item.Value),
                TransactionsLog = ret.TransactionsLog.Select(item => new SimulationTransaction
                {
                    Date = item.Date,
                    Price = item.Price,
                    Action = item.Action,
                    BudgetAfter = item.BudgetAfter,
                    Quantity = item.Quantity,
                    Company = companies.FirstOrDefault(x => x.Id == item.CompanyId)
                }).ToList(),
                StartBudget = model.Budget,
                TotalSimulationValue = ret.SimulationTotalValue,
                PercentageProfit = ret.PercentageProfit,
                MaximalLossOnTransaction = ret.MaximalLossOnTransaction,
                MaximalGainOnTransaction = ret.MaximalGainOnTransaction,
                MaximalSimulationValue = ret.MaximalSimulationValue,
                MinimalSimulationValue = ret.MinimalSimulationValue
            });
        }

        //[HttpGet]
        //public ActionResult Results()
        //{
        //    return View();
        //}

        private async Task<SimulationDto> ConvertViewModelToDto(SimulationViewModel viewModel)
        {
            return new SimulationDto
            {
                StartDate = viewModel.StartDate,
                EndDate = viewModel.EndDate,
                SelectedStrategyId = viewModel.SelectedStrategyId,
                SelectedCompanyIds = viewModel.AllCompanies ?
                    (await _companyService.GetCompanies()).Select(c => c.Id).ToList() :
                    viewModel.SelectedCompanyIds,
                UserId = CurrentUserId,
                Budget = viewModel.Budget
            };
        }

        private async Task<SimulationViewModel> GetViewModel()
        {
            var model = new SimulationViewModel
            {
                Companies = await _companyService.GetCompanies(),
                StartDate = new DateTime(2006, 01, 01),
                EndDate = DateTime.Today, 
                Strategies = await _strategyService.GetStrategies(CurrentUserId)
            };
            return model;
        }
    }
}