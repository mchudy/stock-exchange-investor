using StockExchange.DataAccess.Cache;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IList<Price>> GetCurrentPrices(int days) => 
            await _baseRepository.GetCurrentPrices(days);

        public async Task<Dictionary<Company, List<Price>>> GetPrices(IList<int> companyIds) =>
            await _baseRepository.GetPrices(companyIds);

        public async Task<IList<Price>> GetPrices(int companyId, DateTime endDate) =>
            await _baseRepository.GetPrices(companyId, endDate);

        public async Task<IList<Price>> GetCurrentPrices(IList<int> companyIds)
        {
            // not pretty, but seems to work well in this case
            string cacheKey = CacheKeys.CurrentPrices + "_" + string.Join(",", companyIds.OrderBy(x => x));
            return await Get(cacheKey, async () => await _baseRepository.GetCurrentPrices(companyIds));
        }
    }
}