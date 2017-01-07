using StockExchange.Business.Models.Strategy;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchange.Business.ServiceInterfaces
{
    /// <summary>
    /// Provides methods for operating on trading strategies
    /// </summary>
    public interface IStrategyService
    {
        /// <summary>
        /// Creates a new strategy
        /// </summary>
        /// <param name="strategy">A strategy representation</param>
        /// <returns>ID of the newly created strategy</returns>
        Task<int> CreateStrategy(StrategyDto strategy);

        /// <summary>
        /// Returns the user's strategies
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>The list of strategies</returns>
        Task<IList<StrategyDto>> GetStrategies(int userId);

        /// <summary>
        /// Returns a strategy by ID
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="strategyId">The strategy ID</param>
        /// <returns>The strategy</returns>
        Task<StrategyDto> GetStrategy(int userId, int strategyId);

        /// <summary>
        /// Deletes a strategy
        /// </summary>
        /// <param name="strategyId">The strategy ID</param>
        /// <param name="userId">The user ID</param>
        Task DeleteStrategy(int strategyId, int userId);

        /// <summary>
        /// Updates the strategy
        /// </summary>
        /// <param name="dto">A new strategy data</param>
        Task UpdateStrategy(StrategyDto dto);
    }
}
