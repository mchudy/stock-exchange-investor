using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Repositories
{
    /// <summary>
    /// A database for <see cref="Company"/> entities
    /// </summary>
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="CompanyRepository"/>
        /// </summary>
        /// <param name="model">Database context</param>
        public CompanyRepository(StockExchangeModel model) : base(model)
        { }

        /// <inheritdoc />
        public async Task<IList<Company>> GetCompanies()
        {
            return await DbSet
                .OrderBy(c => c.Code)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IList<Company>> GetCompaniesFromGroup(int groupId)
        {
            return await DbSet
                .Where(g => g.CompanyGroupCompanies.Any(cgc => cgc.CompanyGroupId == groupId))
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IList<string>> GetCompanyNames()
        {
            return await DbSet.Select(c => c.Code)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IList<Company>> GetCompanies(IList<int> ids)
        {
            return await GetQueryable().Where(item => ids.Contains(item.Id))
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IList<CompanyGroup>> GetCompanyGroups()
        {
            return await Context.CompanyGroups
                .Include(g => g.CompanyGroupCompanies)
                .ToListAsync();
        }
    }
}