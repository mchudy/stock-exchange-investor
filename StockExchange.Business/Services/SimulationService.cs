using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using StockExchange.Business.Models.Simulations;

namespace StockExchange.Business.Services
{
    public class SimulationService : ISimulationService
    {
        private readonly IRepository<InvestmentStrategy> _strategiesRepository;
        private readonly IIndicatorsService _indicatorsService;

        public SimulationService(IRepository<InvestmentStrategy> strategiesRepository, IIndicatorsService indicatorsService)
        {
            _strategiesRepository = strategiesRepository;
            _indicatorsService = indicatorsService;
        }

        public SimulationResult RunSimulation(SimulationDto simulationDto)
        {
            return new SimulationResult();
        }
    }
}
