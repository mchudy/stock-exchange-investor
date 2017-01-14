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

        Task<IList<InvestmentStrategy>> GetStrategies(int userId);

        Task<InvestmentStrategy> GetStrategy(int userId, int strategyId);
        Task<bool> StrategyExists(int userId, string name);
    }
}
