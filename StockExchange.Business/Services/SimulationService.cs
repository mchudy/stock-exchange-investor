using System.Linq;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Business.Models.Simulations;

namespace StockExchange.Business.Services
{
    public class SimulationService : ISimulationService
    {
        private readonly IStrategyService _strategyService;
        private readonly IIndicatorsService _indicatorsService;
        private readonly ICompanyService _companyService;

        public SimulationService(IStrategyService strategyService, IIndicatorsService indicatorsService, ICompanyService companyService)
        {
            _strategyService = strategyService;
            _indicatorsService = indicatorsService;
            _companyService = companyService;
        }

        public SimulationResult RunSimulation(SimulationDto simulationDto)
        {
            var strategy = _strategyService.GetUserStrategy(simulationDto.UserId, simulationDto.SelectedStrategyId);
            if (simulationDto.SelectedCompanyIds == null)
                simulationDto.SelectedCompanyIds = _companyService.GetAllCompanies().Select(item => item.Id).ToList();
            foreach (var indicator in strategy.Indicators)
            {
                if (indicator.IndicatorType == null) continue;
                var indicatorValues = _indicatorsService.GetIndicatorValues(indicator.IndicatorType.Value, simulationDto.SelectedCompanyIds);
                foreach (var companyIndicatorValuese in indicatorValues)
                {
                    var signals = _indicatorsService.GetIndicatorSignals(companyIndicatorValuese.IndicatorValues.Where(item => item.Date >= simulationDto.StartDate && item.Date <= simulationDto.EndDate).ToList(),
                        indicator.IndicatorType.Value);
                    foreach (var signal in signals)
                    {
                        
                    }
                }
            }
            return new SimulationResult();
        }
    }
}
