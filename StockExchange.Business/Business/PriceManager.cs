using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using StockExchange.Business.Models;
using StockExchange.Common;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;

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
            return _companyRepository.GetQueryable().Select(item => item.name).Distinct().ToList();
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
                Id = price.id,
                ClosePrice = price.closePrice,
                Date = price.date,
                HighPrice = price.highPrice,
                LowPrice = price.lowPrice,
                OpenPrice = price.openPrice,
                Volume = price.volume,
                CompanyId = price.Company.id,
                CompanyName = price.Company.name
            };
        }
    }
}
