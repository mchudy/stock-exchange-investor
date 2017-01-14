using StockExchange.Business.Extensions;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Paging;
using StockExchange.Business.Models.Price;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StockExchange.Business.Services
{
    /// <summary>
    /// Provides methods for operating on stock prices
    /// </summary>
    public sealed class PriceService : IPriceService
    {
        private readonly IPriceRepository _priceRepository;

        /// <summary>
        /// Creates a new instance of <see cref="PriceService"/>
        /// </summary>
        /// <param name="priceRepository"></param>
        public PriceService(IPriceRepository priceRepository)
        {
            _priceRepository = priceRepository;
        }

        /// <inheritdoc />
        public async Task<PagedList<PriceDto>> GetPrices(PagedFilterDefinition<PriceFilter> pagedFilterDefinition)
        {
            var results = _priceRepository.GetQueryable().Select(GetSelectDtoExpression());
            results = Filter(pagedFilterDefinition.Filter, results);
            results = Search(pagedFilterDefinition.Search, results);
            results = results.Where(pagedFilterDefinition.Searches);
            results = results.OrderBy(pagedFilterDefinition.OrderBys);
            return await results.ToPagedList(pagedFilterDefinition.Start, pagedFilterDefinition.Length);
        }

        /// <inheritdoc />
        public async Task<IList<CompanyPricesDto>> GetPrices(IList<int> companyIds)
        {
            var prices = await _priceRepository.GetPrices(companyIds);
            return prices.Select(g => new CompanyPricesDto
            {
                Company = g.Key,
                Prices = g.Value
            }).ToList();
        }

        /// <inheritdoc />
        public async Task<IList<Price>> GetPrices(int companyId, DateTime endDate)
        {
            return await _priceRepository.GetPrices(companyId, endDate);
        }

        /// <inheritdoc />
        public async Task<IList<Price>> GetCurrentPrices(IList<int> companyIds)
        {
            return await _priceRepository.GetCurrentPrices(companyIds);
        }

        /// <inheritdoc />
        public async Task<IList<Price>> GetCurrentPrices(int days)
        {
            return await _priceRepository.GetCurrentPrices(days);
        }

        /// <inheritdoc />
        public async Task<object> GetFilterValues(FilterDefinition<PriceFilter> filterDefinition, string fieldName)
        {
            var results = _priceRepository.GetQueryable().Select(GetSelectDtoExpression());
            results = Filter(filterDefinition.Filter, results);
            results = Search(filterDefinition.Search, results);
            var values = results.Select(fieldName).Distinct().OrderBy(item => item);
            return await values.ToListAsync();
        }

        //TODO: cache
        /// <inheritdoc />
        public async Task<DateTime> GetMaxDate()
        {
            return await _priceRepository.GetQueryable().MaxAsync(item => item.Date);
        }

        //TODO: cache
        /// <inheritdoc />
        public async Task<PagedList<MostActivePriceDto>> GetAdvancers(PagedFilterDefinition<TransactionFilter> message)
        {
            var dates = await GetTwoMaxDates();
            var date = dates.First();
            var previousDate = dates.Last();
            var prices = await
                _priceRepository.GetQueryable()
                    .Include(p => p.Company)
                    .Where(p => dates.Contains(p.Date)).ToListAsync();
            var ret = prices.Where(p => p.Date == date).Select(p => new MostActivePriceDto
            {
                ClosePrice = p.ClosePrice,
                Volume = p.Volume,
                CompanyName = p.Company.Code,
            }).ToList();
            foreach (var priceDto in ret)
            {
                var firstOrDefault = prices.FirstOrDefault(p => p.Date == previousDate && p.Company.Code == priceDto.CompanyName);
                if (firstOrDefault == null) continue;
                var previousPrice = firstOrDefault.ClosePrice;
                priceDto.Change = (priceDto.ClosePrice - previousPrice) / previousPrice * 100;
            }
            return ret.OrderByDescending(item => item.Change).Where(item => item.Change > 0).ToPagedList(message.Start, message.Length);
        }

        //TODO: cache
        /// <inheritdoc />
        public async Task<PagedList<MostActivePriceDto>> GetDecliners(PagedFilterDefinition<TransactionFilter> message)
        {
            var dates = await GetTwoMaxDates();
            var date = dates.First();
            var previousDate = dates.Last();
            var prices = await
                  _priceRepository.GetQueryable()
                      .Include(p => p.Company)
                      .Where(p => dates.Contains(p.Date)).ToListAsync();
            var ret = prices.Where(p => p.Date == date).Select(p => new MostActivePriceDto
            {
                ClosePrice = p.ClosePrice,
                Volume = p.Volume,
                CompanyName = p.Company.Code,
            }).ToList();
            foreach (var priceDto in ret)
            {
                var firstOrDefault = prices.FirstOrDefault(p => p.Date == previousDate && p.Company.Code == priceDto.CompanyName);
                if (firstOrDefault == null) continue;
                var previousPrice = firstOrDefault.ClosePrice;
                priceDto.Change = (priceDto.ClosePrice - previousPrice) / previousPrice * 100;
            }
            return ret.OrderBy(item => item.Change).Where(item => item.Change < 0).ToPagedList(message.Start, message.Length);
        }

        //TODO: cache
        /// <inheritdoc />
        public async Task<PagedList<MostActivePriceDto>> GetMostActive(PagedFilterDefinition<TransactionFilter> message)
        {
            var dates = await GetTwoMaxDates();
            var date = dates.First();
            var previousDate = dates.Last();
            var prices = await
                  _priceRepository.GetQueryable()
                      .Include(p => p.Company)
                      .Where(p => dates.Contains(p.Date)).ToListAsync();
            var ret = prices.Where(p => p.Date == date).Select(p => new MostActivePriceDto
            {
                ClosePrice = p.ClosePrice,
                Volume = p.Volume,
                CompanyName = p.Company.Code,
            }).ToList();
            foreach (var priceDto in ret)
            {
                var firstOrDefault = prices.FirstOrDefault(p => p.Date == previousDate && p.Company.Code == priceDto.CompanyName);
                if (firstOrDefault != null)
                {
                    var previousPrice = firstOrDefault.ClosePrice;
                    priceDto.Change = (priceDto.ClosePrice - previousPrice) / previousPrice * 100;
                }
                else
                {
                    priceDto.Change = 0;
                }
            }
            return ret.OrderByDescending(item => item.Volume).ToPagedList(message.Start, message.Length);
        }

        //TODO: cache
        private async Task<IList<DateTime>> GetTwoMaxDates()
        {
            return await _priceRepository.GetQueryable()
                .OrderByDescending(item => item.Date)
                .Select(item => item.Date)
                .Distinct()
                .Take(2)
                .ToListAsync();
        }

        private static IQueryable<PriceDto> Filter(PriceFilter filter, IQueryable<PriceDto> results)
        {
            if (filter == null) return results;
            if (filter.StartDate != null)
                results = results.Where(item => item.Date >= filter.StartDate);
            if (filter.EndDate != null)
                results = results.Where(item => item.Date <= filter.EndDate);
            if (!string.IsNullOrWhiteSpace(filter.CompanyName))
                results = results.Where(item => item.CompanyName == filter.CompanyName);
            return results;
        }

        private static IQueryable<PriceDto> Search(string search, IQueryable<PriceDto> results)
        {
            if (!string.IsNullOrWhiteSpace(search))
                results = results.Where(item => item.CompanyName.Contains(search));
            return results;
        }

        private static Expression<Func<Price, PriceDto>> GetSelectDtoExpression()
        {
            return price => new PriceDto
            {
                Id = price.Id,
                ClosePrice = price.ClosePrice,
                Date = price.Date,
                HighPrice = price.HighPrice,
                LowPrice = price.LowPrice,
                OpenPrice = price.OpenPrice,
                Volume = price.Volume,
                CompanyId = price.Company.Id,
                CompanyName = price.Company.Code
            };
        }
    }
}
