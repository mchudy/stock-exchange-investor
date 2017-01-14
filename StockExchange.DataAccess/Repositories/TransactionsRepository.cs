using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Repositories
{
    public class TransactionsRepository : GenericRepository<UserTransaction>, ITransactionsRepository
    {

        public async Task<IList<UserTransaction>> GetAllUserTransactions(int userId)
        {
            return await DbSet
                .Include(t => t.Company)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.Id)
                .ToListAsync();
        }

        public async Task<Dictionary<int, List<UserTransaction>>> GetTransactionsByCompany(int userId)
        {
            return await DbSet
                .Include(t => t.Company)
                .Where(t => t.UserId == userId)
                .GroupBy(t => t.CompanyId)
                .Where(t => t.Sum(tr => tr.Quantity) > 0)
                .ToDictionaryAsync(t => t.Key, t => t.ToList());
        }

        public async Task<int> GetTransactionsCount(int userId)
        {
            return await DbSet.CountAsync(t => t.UserId == userId);
        }
    }
}