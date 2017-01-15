using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Repositories
{
    /// <summary>
    /// A database repository for <see cref="Price"/> entities
    /// </summary>
    public class PriceRepository : GenericRepository<Price>, IPriceRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="PriceRepository"/>
        /// </summary>
        /// <param name="model">Database context</param>
        public PriceRepository(StockExchangeModel model) : base(model)
        {
        }

        /// <inheritdoc />
        public async Task<IList<Price>> GetCurrentPrices(int days)
        {
            return
                await DbSet.GroupBy(p => p.CompanyId, (c, prices) => prices.OrderByDescending(p => p.Date).Take(days))
                    .SelectMany(p => p)
                    .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IList<Price>> GetCurrentPrices(IList<int> companyIds)
        {
            return await DbSet.Where(p => companyIds.Contains(p.CompanyId))
                .GroupBy(p => p.CompanyId, (id, prices) => prices.OrderByDescending(pr => pr.Date).FirstOrDefault())
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Dictionary<Company, List<Price>>> GetPrices(IList<int> companyIds)
        {
            return await DbSet
                .Include(p => p.Company)
                .Where(p => companyIds.Contains(p.CompanyId))
                .GroupBy(p => p.Company)
                .ToDictionaryAsync(g => g.Key, g => g.OrderBy(p => p.Date).ToList());
        }

        /// <inheritdoc />
        public async Task<IList<Price>> GetPrices(int companyId, DateTime endDate)
        {
            return await DbSet
                .Where(p => p.CompanyId == companyId && p.Date <= endDate)
                .OrderBy(item => item.Date)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<DateTime> GetMaxDate()
        {
            return await DbSet.MaxAsync(item => item.Date);
        }

        /// <inheritdoc />
        public async Task<IList<DateTime>> GetTwoMaxDates()
        {
            return await DbSet
                .OrderByDescending(item => item.Date)
                .Select(item => item.Date)
                .Distinct()
                .Take(2)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<List<Price>> GetPricesForDates(IList<DateTime> dates)
        {
            return await DbSet
                .Include(p => p.Company)
                .Where(p => dates.Contains(p.Date))
                .ToListAsync();
        }
    }
}