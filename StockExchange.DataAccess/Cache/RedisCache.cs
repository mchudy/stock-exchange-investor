using log4net;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Cache
{
    /// <summary>
    /// Cache implementation for Redis
    /// </summary>
    public class RedisCache : ICache
    {
        private const string ConnectionStringKey = "RedisConnection";

        private static readonly ILog log = LogManager.GetLogger(typeof(RedisCache));
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize
        };

        private static readonly Lazy<ConnectionMultiplexer> _lazyRedisConnection;
        private static ConnectionMultiplexer _redisConnection => _lazyRedisConnection.Value;
        private static IDatabase _db => _redisConnection.GetDatabase();

        static RedisCache()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
            _lazyRedisConnection  = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString), true);
        }

        /// <inheritdoc />
        public async Task<T> Get<T>(string key) where T : class
        {
            try
            {
                var redisObject = await _db.StringGetAsync(key);
                if (redisObject.HasValue)
                {
                    return JsonConvert.DeserializeObject<T>(redisObject, _serializerSettings);
                }
            }
            catch (RedisConnectionException e)
            {
                log.Error("Redis connection error", e);
            }
            return null;
        }

        /// <inheritdoc />
        public async Task Set<T>(string key, T objectToCache) where T : class
        {
            var serializedObject = JsonConvert.SerializeObject(objectToCache, _serializerSettings);
            try
            {
                await _db.StringSetAsync(key, serializedObject);
            }
            catch (RedisConnectionException e)
            {
                log.Error("Redis connection error", e);
            }
        }

        /// <inheritdoc />
        public async Task<bool> Remove(string key)
        {
            return await _db.KeyDeleteAsync(key);
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public async Task<long> Remove(IEnumerable<string> keys)
        {
            return await _db.KeyDeleteAsync(keys.Cast<RedisKey>().ToArray());
        }
    }
}