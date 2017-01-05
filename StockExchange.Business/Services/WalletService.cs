using StockExchange.Business.Extensions;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Wallet;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IList<OwnedCompanyStocksDto>> GetOwnedStocks(int userId)
        {
            var transactionsByCompany = await _transactionsService.GetTransactionsByCompany(userId);
            var currentPrices = await _priceService.GetCurrentPrices(transactionsByCompany.Keys.ToList());
            return transactionsByCompany
                .Select(entry => BuildCompanyOwnedStocksDto(userId, entry, currentPrices))
                .ToList();
        }

        public async Task<PagedList<OwnedCompanyStocksDto>> GetOwnedStocks(int currentUserId, PagedFilterDefinition<TransactionFilter> searchMessage)
        {
            var transactionsByCompany = await _transactionsService.GetTransactionsByCompany(currentUserId);
            var currentPrices = await _priceService.GetCurrentPrices(transactionsByCompany.Keys.ToList());
            return transactionsByCompany
                .Select(entry => BuildCompanyOwnedStocksDto(currentUserId, entry, currentPrices))
                .ToPagedList(searchMessage.Start, searchMessage.Length);
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
                TotalBuyPrice = transactions.Sum(t => t.Quantity * t.Price),
                AverageBuyPrice = transactions.Sum(t => t.Quantity * t.Price) / ownedStocksCount
            };
        }
    }
}
