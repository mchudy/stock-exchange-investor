using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Paging;
using StockExchange.Business.Models.Wallet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchange.Business.ServiceInterfaces
{
    /// <summary>
    /// Provides methods for operating on user's wallet
    /// </summary>
    public interface IWalletService
    {
        /// <summary>
        /// Returns all stocks owned by the user at the moment
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>A list of owned stocks grouped by company</returns>
        Task<IList<OwnedCompanyStocksDto>> GetOwnedStocks(int userId);

        /// <summary>
        /// Returns a paged list of stocks owned by the user
        /// </summary>
        /// <param name="currentUserId">The user ID</param>
        /// <param name="searchMessage">The paging</param>
        /// <returns>A paged list of stocks owned by the user</returns>
        Task<PagedList<OwnedCompanyStocksDto>> GetOwnedStocks(int currentUserId, PagedFilterDefinition<TransactionFilter> searchMessage);
    }
}