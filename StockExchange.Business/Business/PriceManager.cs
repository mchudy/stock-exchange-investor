using StockExchange.Business.Extensions;
using StockExchange.Business.Models;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace StockExchange.Business.Business
{
    public sealed class PriceManager : IPriceManager
    {
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<Price> _priceRepository;

        public PriceManager(IRepository<Company> companyRepository, IRepository<Price> priceRepository)
        {
            _priceRepository = priceRepository;
            _companyRepository = companyRepository;
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

        public IEnumerable<string> GetCompanyNames()
        {
            return _companyRepository.GetQueryable().Select(item => item.Name).Distinct().ToList();
        }

        public IList<CompanyPricesDto> GetPricesForCompanies(IList<int> companyIds)
        {
            //TODO: move to repository
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
                CompanyName = price.Company.Name
            };
        }
    }
}
