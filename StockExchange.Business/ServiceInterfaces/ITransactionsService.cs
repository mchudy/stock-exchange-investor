using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Paging;
using StockExchange.Business.Models.Transaction;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchange.Business.ServiceInterfaces
{
    /// <summary>
    /// Provides methods for operating on user transactions
    /// </summary>
    public interface ITransactionsService
    {
        /// <summary>
        /// Adds a new user transaction
        /// </summary>
        /// <param name="dto">The transaction data</param>
        Task AddTransaction(UserTransactionDto dto);
        
        /// <summary>
        /// Returns a paged list of transaction for the user
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="filter">The paging data</param>
        /// <returns>The paged list of transactions</returns>
        Task<PagedList<UserTransactionDto>> GetTransactions(int userId, PagedFilterDefinition<TransactionFilter> filter);

        /// <summary>
        /// Returns a total number of transactions concluded by the user
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>A total number of transactions concluded by the user</returns>
        Task<int> GetTransactionsCount(int userId);

        /// <summary>
        /// Returns user's transactions grouped by companies
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>Transactions grouped by companies</returns>
        Task<Dictionary<Company, List<UserTransaction>>> GetTransactionsByCompany(int userId);
    }
}