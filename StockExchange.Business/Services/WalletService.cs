using StockExchange.Business.Models;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace StockExchange.Business.Services
{
    public class WalletService : IWalletService
    {
        private readonly IRepository<UserTransaction> _transactionsRepository;
        private readonly IPriceService _priceService;

        public WalletService(IRepository<UserTransaction> transactionsRepository, IPriceService priceService)
        {
            _transactionsRepository = transactionsRepository;
            _priceService = priceService;
        }

        public IList<OwnedCompanyStocksDto> GetOwnedStocks(int userId)
        {
            var transactionsByCompany = GetTransactionsByCompany(userId);
            var currentPrices = _priceService.GetCurrentPrices(transactionsByCompany.Keys.ToList());

            return transactionsByCompany
                .Select(entry => BuildCompanyOwnedStocksDto(userId, entry, currentPrices))
                .ToList();;
        }

        private OwnedCompanyStocksDto BuildCompanyOwnedStocksDto(int userId, KeyValuePair<int, List<UserTransaction>> entry, IList<Price> currentPrices)
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
                CurrentValue = currentPrice*ownedStocksCount,
                UserId = userId,
                TotalBuyPrice = transactions.Sum(t => t.Quantity*t.Price)
            };
        }

        private Dictionary<int, List<UserTransaction>> GetTransactionsByCompany(int userId)
        {
            return _transactionsRepository.GetQueryable()
                .Include(t => t.Company)
                .Where(t => t.UserId == userId)
                .GroupBy(t => t.CompanyId)
                .Where(t => t.Sum(tr => tr.Quantity) > 0)
                .ToDictionary(t => t.Key, t => t.ToList());
        }
    }
}
