using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Repositories
{
    /// <summary>
    /// A database repository for <see cref="User"/> entities
    /// </summary>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="UserRepository"/>
        /// </summary>
        /// <param name="model">Database context</param>
        public UserRepository(StockExchangeModel model) : base(model)
        { }

        /// <inheritdoc />
        public async Task<User> GetUser(int userId)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.Id == userId);
        }

        /// <inheritdoc />
        public async Task<User> GetUserWithTransactions(int userId)
        {
            return await DbSet.Include(u => u.Transactions)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}