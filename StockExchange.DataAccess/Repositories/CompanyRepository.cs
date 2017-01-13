using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Repositories
{
    /// <summary>
    /// Repository for <see cref="Company"/> entities
    /// </summary>
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        // No need to cache these queries, no real performance gain

        /// <inheritdoc />
        public async Task<IList<Company>> GetCompanies()
        {
            return await DbSet.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IList<string>> GetCompanyNames()
        {
            return await DbSet.Select(c => c.Code).Distinct().ToListAsync();
        }

        public async Task<IList<Company>> GetCompanies(IList<int> ids)
        {
            return await GetQueryable().Where(item => ids.Contains(item.Id)).ToListAsync();
        }
    }
}