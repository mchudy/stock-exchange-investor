using StockExchange.DataAccess.Models;

namespace StockExchange.DataAccess.IRepositories
{
    public interface IStrategiesRepository : IRepository<InvestmentStrategy>
    {
        void DeleteIndicator(StrategyIndicator strategyIndicator);
    }
}
