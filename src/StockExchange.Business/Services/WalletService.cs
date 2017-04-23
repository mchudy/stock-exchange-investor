using StockExchange.Business.Extensions;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Paging;
using StockExchange.Business.Models.Wallet;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.Cache;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.Business.Services
{
    /// <summary>
    /// Provides methods for operating on user's wallet
    /// </summary>
    public class WalletService : IWalletService
    {
        private readonly ITransactionsService _transactionsService;
        private readonly IPriceService _priceService;
        private readonly ICache _cache;

        /// <summary>
        /// Creates a new <see cref="WalletService"/> instance
        /// </summary>
        /// <param name="transactionsService"></param>
        /// <param name="priceService"></param>
        /// <param name="cache"></param>
        public WalletService(ITransactionsService transactionsService, IPriceService priceService, ICache cache)
        {
            _transactionsService = transactionsService;
            _priceService = priceService;
            _cache = cache;
        }

        //TODO: refactor cache usage
        /// <inheritdoc />
        public async Task<IList<OwnedCompanyStocksDto>> GetOwnedStocks(int userId)
        {
            var ownedStocks = await _cache.Get<IList<OwnedCompanyStocksDto>>(CacheKeys.OwnedStocks(userId));
            if (ownedStocks != null)
                return ownedStocks;

            var transactionsByCompany = await _transactionsService.GetTransactionsByCompany(userId);
            var currentPrices = await _priceService.GetCurrentPrices(transactionsByCompany.Keys.Select(k => k.Id).ToList());
            ownedStocks = transactionsByCompany
                .Select(entry => BuildCompanyOwnedStocksDto(userId, entry, currentPrices))
                .ToList();
            await _cache.Set(CacheKeys.OwnedStocks(userId), ownedStocks);
            return ownedStocks;
        }

        /// <inheritdoc />
        public async Task<PagedList<OwnedCompanyStocksDto>> GetOwnedStocks(int currentUserId, PagedFilterDefinition<TransactionFilter> searchMessage)
        {
            var transactionsByCompany = await _transactionsService.GetTransactionsByCompany(currentUserId);
            var currentPrices = await _priceService.GetCurrentPrices(transactionsByCompany.Keys.Select(k => k.Id).ToList());
            var ret = transactionsByCompany.Select(entry => BuildCompanyOwnedStocksDto(currentUserId, entry, currentPrices));

            if (!string.IsNullOrWhiteSpace(searchMessage.Search))
                ret = ret.Where(item => item.CompanyName.Contains(searchMessage.Search.ToUpper()));

            ret = ret.OrderBy(searchMessage.OrderBys);

            return ret.ToPagedList(searchMessage.Start, searchMessage.Length);
        }

        private static OwnedCompanyStocksDto BuildCompanyOwnedStocksDto(int userId, KeyValuePair<Company, List<UserTransaction>> entry, IEnumerable<Price> currentPrices)
        {
            var transactions = entry.Value;
            decimal currentPrice = currentPrices.FirstOrDefault(p => p.CompanyId == entry.Key.Id)?.ClosePrice ?? 0;
            int ownedStocksCount = transactions.Sum(t => t.Quantity);
            return new OwnedCompanyStocksDto
            {
                CompanyId = entry.Key.Id,
                CompanyName = entry.Key.Code,
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
