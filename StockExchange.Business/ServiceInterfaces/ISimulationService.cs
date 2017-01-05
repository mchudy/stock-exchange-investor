using System.Threading.Tasks;
using StockExchange.Business.Models.Simulations;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface ISimulationService
    {
        Task<SimulationResultDto> RunSimulation(SimulationDto simulationDto);
    }
}
