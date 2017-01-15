using StockExchange.DataAccess.Cache;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.CachedRepositories
{
    public class CachedPriceRepository : CachedRepositoryBase<Price>, IPriceRepository
    {
        private readonly IPriceRepository _baseRepository;

        public CachedPriceRepository(IPriceRepository baseRepository, ICache cache) 
            : base(baseRepository, cache)
        {
            _baseRepository = baseRepository;
        }

        public async Task<IList<Price>> GetCurrentPrices(int days)
        {
            return await _baseRepository.GetCurrentPrices(days);
        }

        public async Task<Dictionary<Company, List<Price>>> GetPrices(IList<int> companyIds)
        {
           return await _baseRepository.GetPrices(companyIds);
        }

        public async Task<IList<Price>> GetPrices(int companyId, DateTime endDate)
        {
            return await _baseRepository.GetPrices(companyId, endDate);
        }

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

        public async Task<IList<DateTime>> GetTwoMaxDates()
        {
            return await Get(CacheKeys.TwoMaxDates, 
                async () => await _baseRepository.GetTwoMaxDates());
        }

        public async Task<List<Price>> GetPricesForDates(IList<DateTime> dates)
        {
            //string cacheKey = CacheKeys.PricesForDate + "_" + string.Join(",", dates.OrderBy(x => x)
            //    .Select(d => d.ToShortDateString()));
            //return await Get(cacheKey, async () => await _baseRepository.GetPricesForDates(dates));
            return await _baseRepository.GetPricesForDates(dates);
        }

        public async Task<IList<Price>> GetCurrentPrices(IList<int> companyIds)
        {
            return await Get(CacheKeys.CurrentPrices(companyIds),
                async () => await _baseRepository.GetCurrentPrices(companyIds));
        }
    }
}