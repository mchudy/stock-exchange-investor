using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Repositories
{
    public class PriceRepository : GenericRepository<Price>, IPriceRepository
    {
        public async Task<IList<Price>> GetCurrentPrices(int days)
        {
            return await DbSet.GroupBy(p => p.CompanyId, (c, prices) => prices.OrderByDescending(p => p.Date).Take(days))
                .SelectMany(p => p)
                .ToListAsync();
        }

        public async Task<IList<Price>> GetCurrentPrices(IList<int> companyIds)
        {
            return await DbSet.Where(p => companyIds.Contains(p.CompanyId))
                .GroupBy(p => p.CompanyId, (id, prices) => prices.OrderByDescending(pr => pr.Date).FirstOrDefault())
                .ToListAsync();
        }
    }
}