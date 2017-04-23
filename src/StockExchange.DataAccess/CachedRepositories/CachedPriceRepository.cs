using StockExchange.DataAccess.Cache;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using StockExchange.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.CachedRepositories
{
    /// <summary>
    /// Decorator for <see cref="PriceRepository"/> which uses cache
    /// </summary>
    public class CachedPriceRepository : CachedRepositoryBase<Price>, IPriceRepository
    {
        private readonly IPriceRepository _baseRepository;

        /// <summary>
        /// Creates a new instance of <see cref="CachedPriceRepository"/>
        /// </summary>
        /// <param name="baseRepository">Database repository</param>
        /// <param name="cache">Cache implementation</param>
        public CachedPriceRepository(IPriceRepository baseRepository, ICache cache) 
            : base(baseRepository, cache)
        {
            _baseRepository = baseRepository;
        }

        /// <inheritdoc />
        public async Task<IList<Price>> GetCurrentPrices(int days)
        {
            return await _baseRepository.GetCurrentPrices(days);
        }

        /// <inheritdoc />
        public async Task<Dictionary<Company, List<Price>>> GetPrices(IList<int> companyIds)
        {
           return await _baseRepository.GetPrices(companyIds);
        }

        /// <inheritdoc />
        public async Task<IList<Price>> GetPrices(int companyId, DateTime endDate)
        {
            return await _baseRepository.GetPrices(companyId, endDate);
        }

        /// <inheritdoc />
        public async Task<DateTime> GetMaxDate()
        {
            var value = await _cache.Get<DateTime?>(CacheKeys.MaxDate);
            if (value == null)
            {
                value = await _baseRepository.GetMaxDate();
                await _cache.Set(CacheKeys.MaxDate, value);
            }
            return (DateTime)value;
        }

        /// <inheritdoc />
        public async Task<IList<DateTime>> GetTwoMaxDates()
        {
            return await Get(CacheKeys.TwoMaxDates, 
                async () => await _baseRepository.GetTwoMaxDates());
        }

        /// <inheritdoc />
        public async Task<List<Price>> GetPricesForDates(IList<DateTime> dates)
        {
            //string cacheKey = CacheKeys.PricesForDate + "_" + string.Join(",", dates.OrderBy(x => x)
            //    .Select(d => d.ToShortDateString()));
            //return await Get(cacheKey, async () => await _baseRepository.GetPricesForDates(dates));
            return await _baseRepository.GetPricesForDates(dates);
        }

        /// <inheritdoc />
        public async Task<IList<Price>> GetCurrentPrices(IList<int> companyIds)
        {
            return await Get(CacheKeys.CurrentPrices(companyIds),
                async () => await _baseRepository.GetCurrentPrices(companyIds));
        }
    }
}