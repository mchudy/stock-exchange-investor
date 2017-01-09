using StockExchange.DataAccess.Models;

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
    }
}
