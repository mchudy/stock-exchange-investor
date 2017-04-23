using StockExchange.Business.Models.Simulations;
using System.Threading.Tasks;

namespace StockExchange.Business.ServiceInterfaces
{
    /// <summary>
    /// Provides methods for running trading game simulations
    /// </summary>
    public interface ISimulationService
    {
        /// <summary>
        /// Runs a simulation
        /// </summary>
        /// <param name="simulationDto">A DTO representing the simulation</param>
        /// <returns>The simulation results</returns>
        Task<SimulationResultDto> RunSimulation(SimulationDto simulationDto);
    }
}
