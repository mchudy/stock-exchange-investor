using System.Collections.Generic;
using System.Threading.Tasks;
using StockExchange.Business.Extensions;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Wallet;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface IWalletService
    {
        Task<IList<OwnedCompanyStocksDto>> GetOwnedStocks(int userId);

        Task<PagedList<OwnedCompanyStocksDto>> GetOwnedStocks(int currentUserId, PagedFilterDefinition<TransactionFilter> searchMessage);
    }
}