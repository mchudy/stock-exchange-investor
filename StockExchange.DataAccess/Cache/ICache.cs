using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Cache
{
    /// <summary>
    /// General representation of a cache
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Retrieves a value from the cache
        /// </summary>
        /// <typeparam name="T">Type of object to get</typeparam>
        /// <param name="key">Cache key</param>
        /// <returns>Deserialized object from cache</returns>
        Task<T> Get<T>(string key) where T : class;

        /// <summary>
        /// Saves an object in cache
        /// </summary>
        /// <typeparam name="T">Type of object to cache</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="objectToCache">Object to cache</param>
        Task Set<T>(string key, T objectToCache) where T : class;

        /// <summary>
        /// Removes a key-value pair from the cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>Whether the operation succeded</returns>
        Task<bool> Remove(string key);

        /// <summary>
        /// Removes multiple key-value pairs from the cache
        /// </summary>
        /// <param name="keys">Cache keys</param>
        /// <returns>The number of items removed</returns>
        Task<long> Remove(IEnumerable<string> keys);

    }
}