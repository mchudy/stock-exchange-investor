using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Linq;

namespace StockExchange.DataAccess.Repositories
{
    public class StrategiesRepository : GenericRepository<InvestmentStrategy>, IStrategiesRepository
    {
        public void DeleteIndicator(StrategyIndicator strategyIndicator)
        {
            foreach (var property in strategyIndicator.Properties.ToList())
            {
                Context.StrategyIndicatorProperties.Remove(property);
            }
            Context.StrategyIndicators.Remove(strategyIndicator);
        }
    }
}
