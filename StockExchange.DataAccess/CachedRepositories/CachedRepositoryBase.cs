using StockExchange.DataAccess.Cache;
using StockExchange.DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.CachedRepositories
{
    /// <summary>
    /// Base class for repositories operating on cache which uses the decorator pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CachedRepositoryBase<T> : IRepository<T> where T : class 
    {
        private readonly IRepository<T> _baseRepository;

        /// <summary>
        /// Abstract cache representation
        /// </summary>
        protected readonly ICache _cache;

        /// <summary>
        /// Creates a new instance of <see cref="CachedRepositoryBase{T}"/>
        /// </summary>
        /// <param name="baseRepository">Database repository</param>
        /// <param name="cache">Cache</param>
        protected CachedRepositoryBase(IRepository<T> baseRepository, ICache cache)
        {
            _baseRepository = baseRepository;
            _cache = cache;
        }

        /// <inheritdoc />
        public IQueryable<T> GetQueryable() => _baseRepository.GetQueryable();

        /// <inheritdoc />
        public void BulkInsert(IEnumerable<T> entities) => _baseRepository.BulkInsert(entities);

        /// <inheritdoc />
        public void Insert(T entity) => _baseRepository.Insert(entity);

        /// <inheritdoc />
        public void Remove(T entity) => _baseRepository.Remove(entity);

        /// <inheritdoc />
        public async Task<int> RemoveRange(IQueryable<T> entities) => 
            await _baseRepository.RemoveRange(entities);

        /// <inheritdoc />
        public Task<int> Save() => _baseRepository.Save();

        /// <inheritdoc />
        public void Dispose() => _baseRepository.Dispose();

        /// <summary>
        /// Returns a value from cache, if it doesn't exists uses a fallback
        /// and saves the value in cache
        /// </summary>
        /// <typeparam name="TValue">Type of object to retrieve</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="fallback">Method returning the value</param>
        /// <returns>The value from cache</returns>
        protected async Task<TValue> Get<TValue>(string key, Func<Task<TValue>> fallback) where TValue : class
        {
            var value = await _cache.Get<TValue>(key);
            if (value == null)
            {
                value = await fallback();
                await _cache.Set(key, value);
            }
            return value;
        }
    }
}