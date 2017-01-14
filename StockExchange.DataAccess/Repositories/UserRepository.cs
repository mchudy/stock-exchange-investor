using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public async Task<User> GetUserWithTransactions(int userId)
        {
            return await DbSet.Include(u => u.Transactions)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}