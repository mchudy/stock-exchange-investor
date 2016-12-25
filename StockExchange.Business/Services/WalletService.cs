using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using StockExchange.Business.Models.Wallet;

namespace StockExchange.Business.Services
{
    public class WalletService : IWalletService
    {
        private readonly ITransactionsService _transactionsService;
        private readonly IPriceService _priceService;

        public WalletService(ITransactionsService transactionsService, IPriceService priceService)
        {
            _transactionsService = transactionsService;
            _priceService = priceService;
        }

        public IList<OwnedCompanyStocksDto> GetOwnedStocks(int userId)
        {
            var transactionsByCompany = _transactionsService.GetTransactionsByCompany(userId);
            var currentPrices = _priceService.GetCurrentPrices(transactionsByCompany.Keys.ToList());
            return transactionsByCompany
                .Select(entry => BuildCompanyOwnedStocksDto(userId, entry, currentPrices))
                .ToList();
        }

        private static OwnedCompanyStocksDto BuildCompanyOwnedStocksDto(int userId, KeyValuePair<int, List<UserTransaction>> entry, IEnumerable<Price> currentPrices)
        {
            var transactions = entry.Value;
            decimal currentPrice = currentPrices.FirstOrDefault(p => p.CompanyId == entry.Key)?.ClosePrice ?? 0;
            int ownedStocksCount = transactions.Sum(t => t.Quantity);
            return new OwnedCompanyStocksDto
            {
                CompanyId = entry.Key,
                CompanyName = transactions.FirstOrDefault()?.Company?.Code,
                CurrentPrice = currentPrice,
                OwnedStocksCount = ownedStocksCount,
                CurrentValue = currentPrice * ownedStocksCount,
                UserId = userId,
                TotalBuyPrice = transactions.Sum(t => t.Quantity * t.Price)
            };
        }
    }
}
