using StockExchange.DataAccess.Cache;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.CachedRepositories
{
    public class CachedTransactionRepository : CachedRepositoryBase<UserTransaction>, ITransactionsRepository
    {
        private readonly ITransactionsRepository _baseRepository;

        public CachedTransactionRepository(ITransactionsRepository baseRepository, ICache cache) : base(baseRepository, cache)
        {
            _baseRepository = baseRepository;
        }

        public async Task<IList<UserTransaction>> GetAllUserTransactions(int userId)
        {
            return await _baseRepository.GetAllUserTransactions(userId);
        }

        public async Task<Dictionary<int, List<UserTransaction>>> GetTransactionsByCompany(int userId)
        {
            return await _baseRepository.GetTransactionsByCompany(userId);
        }

        public async Task<int> GetTransactionsCount(int userId)
        {
            var value = await _cache.Get<int?>(CacheKeys.TransactionsCount(userId));
            if (value == null)
            {
                value = await _baseRepository.GetTransactionsCount(userId);
                await _cache.Set(CacheKeys.TransactionsCount(userId), value);
            }
            return value.Value;
        }

        public async Task ClearTransactionsCache(int userId)
        {
            await _cache.Remove(new [] 
            {
                CacheKeys.OwnedStocks(userId),
                CacheKeys.TransactionsCount(userId)
            });
        }
    }
}