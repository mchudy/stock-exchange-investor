using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Repositories
{
    /// <summary>
    /// A repository for <see cref="InvestmentStrategy"/> entities
    /// </summary>
    public class StrategiesRepository : GenericRepository<InvestmentStrategy>, IStrategiesRepository
    {
        /// <inheritdoc />
        public void DeleteIndicator(StrategyIndicator strategyIndicator)
        {
            foreach (var property in strategyIndicator.Properties.ToList())
            {
                Context.StrategyIndicatorProperties.Remove(property);
            }
            Context.StrategyIndicators.Remove(strategyIndicator);
        }

        public async Task<IList<InvestmentStrategy>> GetStrategies(int userId)
        {
            return await DbSet.Include(t => t.Indicators)
                .Where(t => t.UserId == userId && !t.IsDeleted)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<InvestmentStrategy> GetStrategy(int userId, int strategyId)
        {
            return await DbSet.FirstOrDefaultAsync(item => item.Id == strategyId 
                && item.UserId == userId && !item.IsDeleted);
        }

        public async Task<bool> StrategyExists(int userId, string name)
        {
            return await DbSet.AnyAsync(s => s.UserId == userId && s.Name == name && !s.IsDeleted);
        }
    }
}
