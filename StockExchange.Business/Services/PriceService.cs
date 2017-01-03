using StockExchange.Business.Extensions;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Price;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace StockExchange.Business.Services
{
    public sealed class PriceService : IPriceService
    {
        private readonly IRepository<Price> _priceRepository;

        public PriceService(IRepository<Price> priceRepository)
        {
            _priceRepository = priceRepository;
        }

        public PagedList<PriceDto> Get(PagedFilterDefinition<PriceFilter> pagedFilterDefinition)
        {
            var results = _priceRepository.GetQueryable().Select(GetSelectDtoExpression());
            results = Filter(pagedFilterDefinition.Filter, results);
            results = Search(pagedFilterDefinition.Search, results);
            results = results.Where(pagedFilterDefinition.Searches);
            results = results.OrderBy(pagedFilterDefinition.OrderBys);
            return results.ToPagedList(pagedFilterDefinition.Start, pagedFilterDefinition.Length);
        }

        public object GetValues(FilterDefinition<PriceFilter> filterDefinition, string fieldName)
        {
            var results = _priceRepository.GetQueryable().Select(GetSelectDtoExpression());
            results = Filter(filterDefinition.Filter, results);
            results = Search(filterDefinition.Search, results);
            var values = results.Select(fieldName).Distinct().OrderBy(item => item);
            return values.ToList();
        }

        public IList<CompanyPricesDto> GetPricesForCompanies(IList<int> companyIds)
        {
            return _priceRepository.GetQueryable()
                .Include(p => p.Company)
                .Where(p => companyIds.Contains(p.CompanyId))
                .GroupBy(p => p.Company)
                .Select(g => new CompanyPricesDto
                {
                    Company = g.Key,
                    Prices = g.OrderBy(p => p.Date).ToList()
                })
                .ToList();
        }

        public IList<Price> GetCurrentPrices(IList<int> companyIds)
        {
            return _priceRepository.GetQueryable()
                .Where(p => companyIds.Contains(p.CompanyId))
                .GroupBy(p => p.CompanyId, (id, prices) => prices.OrderByDescending(pr => pr.Date).FirstOrDefault())
                .ToList();
        }

        public IList<Price> GetPrices(int companyId, DateTime endDate)
        {
            return _priceRepository.GetQueryable()
                .Where(p => p.CompanyId == companyId && p.Date <= endDate)
                .OrderBy(item => item.Date)
                .ToList();
        }

        public IList<Price> GetLastPricesForAllCompanies()
        {
            return _priceRepository.GetQueryable().Where(p => p.Date > DateTime.Today.AddDays(-100)).ToList();
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
