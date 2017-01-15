using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.IRepositories
{
    /// <summary>
    /// A repository for <see cref="UserTransaction"/> entities
    /// </summary>
    public interface ITransactionsRepository : IRepository<UserTransaction>
    {
        /// <summary>
        /// Returns all transactions concluded by the given user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>List of transactions</returns>
        Task<IList<UserTransaction>> GetAllUserTransactions(int userId);

        /// <summary>
        /// Returns a dictionary with all companies and transactions concluded by 
        /// the given user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Dictionary which keys are companies and values are lists
        /// of transactions</returns>
        Task<Dictionary<Company, List<UserTransaction>>> GetTransactionsByCompany(int userId);

        /// <summary>
        /// Returns the total number of transactions concluded by the user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>List of transactions</returns>
        Task<int> GetTransactionsCount(int userId);

        /// <summary>
        /// Clear user cache connected to concluded transactions
        /// </summary>
        /// <param name="userId">User id</param>
        Task ClearTransactionsCache(int userId);
    }
}