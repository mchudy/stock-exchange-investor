using StockExchange.DataAccess.Cache;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using StockExchange.DataAccess.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.CachedRepositories
{
    /// <summary>
    /// Decorator for <see cref="TransactionsRepository"/> which uses cache
    /// </summary>
    public class CachedTransactionRepository : CachedRepositoryBase<UserTransaction>, ITransactionsRepository
    {
        private readonly ITransactionsRepository _baseRepository;

        /// <summary>
        /// Creates a new instance of <see cref="CachedTransactionRepository"/>
        /// </summary>
        /// <param name="baseRepository">Database repository</param>
        /// <param name="cache">Cache implementation</param>
        public CachedTransactionRepository(ITransactionsRepository baseRepository, ICache cache) : base(baseRepository, cache)
        {
            _baseRepository = baseRepository;
        }

        /// <inheritdoc />
        public async Task<UserTransaction> GetTransaction(int id)
        {
            return await _baseRepository.GetTransaction(id);
        }

        /// <inheritdoc />
        public async Task<IList<UserTransaction>> GetAllUserTransactions(int userId)
        {
            return await _baseRepository.GetAllUserTransactions(userId);
        }

        /// <inheritdoc />
        public async Task<Dictionary<Company, List<UserTransaction>>> GetTransactionsByCompany(int userId)
        {
            return await _baseRepository.GetTransactionsByCompany(userId);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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