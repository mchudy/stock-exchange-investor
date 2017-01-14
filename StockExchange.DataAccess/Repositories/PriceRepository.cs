using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Repositories
{
    public class PriceRepository : GenericRepository<Price>, IPriceRepository
    {
        public PriceRepository(StockExchangeModel model) : base(model)
        { }

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

        public async Task<Dictionary<Company, List<Price>>> GetPrices(IList<int> companyIds)
        {
            return await DbSet
                .Include(p => p.Company)
                .Where(p => companyIds.Contains(p.CompanyId))
                .GroupBy(p => p.Company)
                .ToDictionaryAsync(g => g.Key, g => g.OrderBy(p => p.Date).ToList());
        }

        public async Task<IList<Price>> GetPrices(int companyId, DateTime endDate)
        {
            return await DbSet
                .Where(p => p.CompanyId == companyId && p.Date <= endDate)
                .OrderBy(item => item.Date)
                .ToListAsync();
        }
    }
}