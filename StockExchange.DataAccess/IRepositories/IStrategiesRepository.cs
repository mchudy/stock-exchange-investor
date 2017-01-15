using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.IRepositories
{
    /// <summary>
    /// A repository for <see cref="InvestmentStrategy"/> entities
    /// </summary>
    public interface IStrategiesRepository : IRepository<InvestmentStrategy>
    {
        /// <summary>
        /// Removes an indicator for the database
        /// </summary>
        /// <param name="strategyIndicator">An indicator to remove</param>
        void DeleteIndicator(StrategyIndicator strategyIndicator);

        /// <summary>
        /// Returns strategies defined by the user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>List of strategies</returns>
        Task<IList<InvestmentStrategy>> GetStrategies(int userId);

        /// <summary>
        /// Returns the investment strategy
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="strategyId">Strategy id</param>
        /// <returns>The strategy</returns>
        Task<InvestmentStrategy> GetStrategy(int userId, int strategyId);

        /// <summary>
        /// Returns a value indicating whether the strategy with the given name
        /// already exists for the given user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="name">Strategy name</param>
        /// <returns>The value indicating whether the strategy exists</returns>
        Task<bool> StrategyExists(int userId, string name);
    }
}
