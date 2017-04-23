using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Cache
{
    /// <summary>
    /// A fake implementation of cache
    /// </summary>
    public class NoCache : ICache
    {
        /// <inheritdoc />
        public async Task<T> Get<T>(string key)
        {
            return await Task.FromResult(default(T));
        }

        /// <inheritdoc />
        public async Task Set<T>(string key, T objectToCache)
        {
            await Task.FromResult(0);
        }

        /// <inheritdoc />
        public async Task<bool> Remove(string key)
        {
            return await Task.FromResult(true);
        }

        /// <inheritdoc />
        public async Task<long> Remove(IEnumerable<string> keys)
        {
            return await Task.FromResult(keys.Count());
        }

        /// <inheritdoc />
        public async Task Flush()
        {
            await Task.FromResult(0);
        }
    }
}
