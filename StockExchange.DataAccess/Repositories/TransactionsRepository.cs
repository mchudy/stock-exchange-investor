using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Repositories
{
    /// <summary>
    /// A database repository for <see cref="UserTransaction"/> entities
    /// </summary>
    public class TransactionsRepository : GenericRepository<UserTransaction>, ITransactionsRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="TransactionsRepository"/>
        /// </summary>
        /// <param name="model">Database context</param>
        public TransactionsRepository(StockExchangeModel model) : base(model)
        { }

        /// <inheritdoc />
        public async Task<UserTransaction> GetTransaction(int id)
        {
            return await DbSet.FirstOrDefaultAsync(t => t.Id == id);
        }

        /// <inheritdoc />
        public async Task<IList<UserTransaction>> GetAllUserTransactions(int userId)
        {
            return await DbSet
                .Include(t => t.Company)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.Id)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Dictionary<Company, List<UserTransaction>>> GetTransactionsByCompany(int userId)
        {
            return await DbSet
                .Include(t => t.Company)
                .Where(t => t.UserId == userId)
                .GroupBy(t => t.Company)
                .Where(t => t.Sum(tr => tr.Quantity) > 0)
                .ToDictionaryAsync(t => t.Key, t => t.ToList());
        }

        /// <inheritdoc />
        public async Task<int> GetTransactionsCount(int userId)
        {
            return await DbSet.CountAsync(t => t.UserId == userId);
        }

        /// <inheritdoc />
        public Task ClearTransactionsCache(int userId)
        {
            return Task.FromResult(0);
        }
    }
}