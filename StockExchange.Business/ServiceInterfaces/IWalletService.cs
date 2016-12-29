using System.Collections.Generic;
using StockExchange.Business.Extensions;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Wallet;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface IWalletService
    {
        IList<OwnedCompanyStocksDto> GetOwnedStocks(int userId);

        PagedList<OwnedCompanyStocksDto> GetOwnedStocks(int currentUserId, PagedFilterDefinition<TransactionFilter> searchMessage);
    }
}