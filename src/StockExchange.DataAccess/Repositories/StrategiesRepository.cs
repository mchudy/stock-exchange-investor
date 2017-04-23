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
        /// <summary>
        /// Creates a new instance of <see cref="StrategiesRepository"/>
        /// </summary>
        /// <param name="model">Database context</param>
        public StrategiesRepository(StockExchangeModel model) : base(model)
        { }

        /// <inheritdoc />
        public void DeleteIndicator(StrategyIndicator strategyIndicator)
        {
            foreach (var property in strategyIndicator.Properties.ToList())
            {
                Context.StrategyIndicatorProperties.Remove(property);
            }
            Context.StrategyIndicators.Remove(strategyIndicator);
        }

        /// <inheritdoc />
        public async Task<IList<InvestmentStrategy>> GetStrategies(int userId)
        {
            return await DbSet.Include(t => t.Indicators)
                .Where(t => t.UserId == userId && !t.IsDeleted)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<InvestmentStrategy> GetStrategy(int userId, int strategyId)
        {
            return await DbSet.FirstOrDefaultAsync(item => item.Id == strategyId 
                && item.UserId == userId && !item.IsDeleted);
        }

        /// <inheritdoc />
        public async Task<bool> StrategyExists(int userId, string name)
        {
            return await DbSet.AnyAsync(s => s.UserId == userId && s.Name == name && !s.IsDeleted);
        }
    }
}
