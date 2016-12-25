using System.Collections.Generic;
using StockExchange.Business.Models.Wallet;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface IWalletService
    {
        IList<OwnedCompanyStocksDto> GetOwnedStocks(int userId);
    }
}