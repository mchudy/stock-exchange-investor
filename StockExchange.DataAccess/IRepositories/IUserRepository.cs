using StockExchange.DataAccess.Models;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.IRepositories
{
    /// <summary>
    /// A repository for <see cref="User"/> entities
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Returns the user with the given id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User with the given id</returns>
        Task<User> GetUser(int userId);


        /// <summary>
        /// Returns the user with the given id with included transaction data
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User with the given id</returns>
        Task<User> GetUserWithTransactions(int userId);
    }
}