using StockExchange.Business.Models.Simulations;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface ISimulationService
    {
        SimulationResultDto RunSimulation(SimulationDto simulationDto);
    }
}
