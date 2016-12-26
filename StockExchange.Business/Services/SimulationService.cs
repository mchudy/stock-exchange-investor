using StockExchange.Business.ServiceInterfaces;
using StockExchange.Business.Models.Simulations;

namespace StockExchange.Business.Services
{
    public class SimulationService : ISimulationService
    {
        private readonly IStrategyService _strategyService;
        private readonly IIndicatorsService _indicatorsService;

        public SimulationService(IStrategyService strategyService, IIndicatorsService indicatorsService)
        {
            _strategyService = strategyService;
            _indicatorsService = indicatorsService;
        }

        public SimulationResult RunSimulation(SimulationDto simulationDto)
        {
            var strategy = _strategyService.GetUserStrategy(simulationDto.UserId, simulationDto.SelectedStrategyId);
            foreach (var indicator in strategy.Indicators)
            {
                if (indicator.IndicatorType == null) continue;
                var indicatorValues = _indicatorsService.GetIndicatorValues(indicator.IndicatorType.Value, simulationDto.SelectedCompanyIds);
                foreach (var companyIndicatorValuese in indicatorValues)
                {
                    var signals = _indicatorsService.GetIndicatorSignals(companyIndicatorValuese.IndicatorValues,
                        indicator.IndicatorType.Value);
                }
            }
            return new SimulationResult();
        }
    }
}
