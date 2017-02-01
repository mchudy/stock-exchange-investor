using StockExchange.Business.Models.Simulations;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Models.Simulation;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StockExchange.Web.Controllers
{
    /// <summary>
    /// Controller for simulations actions
    /// </summary>
    [Authorize]
    public class SimulationsController : BaseController
    {
        private readonly IStrategyService _strategyService;
        private readonly ICompanyService _companyService;
        private readonly ISimulationService _simulationService;

        /// <summary>
        /// Creates a new instance of <see cref="SimulationsController"/>
        /// </summary>
        /// <param name="strategyService"></param>
        /// <param name="companyService"></param>
        /// <param name="simulationService"></param>
        public SimulationsController(IStrategyService strategyService, ICompanyService companyService, ISimulationService simulationService)
        {
            _strategyService = strategyService;
            _companyService = companyService;
            _simulationService = simulationService;
        }

        /// <summary>
        /// Returns the run simulation view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> RunSimulation()
        {
            var model = await GetViewModel();
            return View(model);
        }

        /// <summary>
        /// Runs the simulation and returns the results view
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> RunSimulation(SimulationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Strategies = await _strategyService.GetStrategies(CurrentUserId);
                model.Companies = await _companyService.GetCompanies();
                model.CompanyGroups = await _companyService.GetCompanyGroups();
                return View(model);
            }

            var results = await _simulationService.RunSimulation(await ConvertViewModelToDto(model));
            var resultsModel = await BuildSimulationResultViewModel(model, results);
            return View("Results", resultsModel);
        }

        private async Task<SimulationResultViewModel> BuildSimulationResultViewModel(SimulationViewModel model, SimulationResultDto ret)
        {
            var ids = ret.CurrentCompanyQuantity.Keys.ToList();
            ids.AddRange(ret.TransactionsLog.Select(item => item.CompanyId));
            var companies = await _companyService.GetCompanies(ids);
            return new SimulationResultViewModel
            {
                CurrentCompanyQuantity = ret.CurrentCompanies
                  ,
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
                AverageLossOnTransaction = ret.TransactionStatistics.AverageLossOnTransaction,
                AverageGainOnTransaction = ret.TransactionStatistics.AverageGainOnTransaction,
                MaximalSimulationValue = ret.MaximalSimulationValue,
                MinimalSimulationValue = ret.MinimalSimulationValue,
                SuccessTransactionPercentage = ret.TransactionStatistics.SuccessTransactionPercentage,
                FailedTransactionPercentage = ret.TransactionStatistics.FailedTransactionPercentage,
                KeepStrategyProfit = (double) ret.KeepStrategyProfit
            };
        }

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
                Budget = viewModel.Budget,
                HasMaximalTransactionLimit = viewModel.HasMaximalTransactonLimit,
                MaximalBudgetPerTransaction = viewModel.MaximalBudgetPerTransaction,
                HasMinimalTransactionLimit = viewModel.HasMinimalTransactionLimit,
                MinimalBudgetPerTransaction = viewModel.MinimalBudgetPerTransaction
            };
        }

        private async Task<SimulationViewModel> GetViewModel()
        {
            var model = new SimulationViewModel
            {
                Companies = await _companyService.GetCompanies(),
                StartDate = new DateTime(2006, 01, 01),
                EndDate = DateTime.Today,
                Strategies = await _strategyService.GetStrategies(CurrentUserId),
                CompanyGroups = await _companyService.GetCompanyGroups()
            };
            return model;
        }
    }
}