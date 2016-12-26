using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
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
                
            }
            return new SimulationResult();
        }
    }
}
