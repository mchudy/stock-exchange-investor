using StockExchange.Business.Models;
using System.Collections.Generic;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface IWalletService
    {
        IList<OwnedCompanyStocksDto> GetOwnedStocks(int userId);
    }
}