using StockExchange.Business.Models.Simulations;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface ISimulationService
    {
        SimulationResult RunSimulation(SimulationDto simulationDto);
    }
}
