using StockExchange.DataAccess.Models;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserWithTransactions(int userId);
        Task<User> GetUser(int userId);
    }
}