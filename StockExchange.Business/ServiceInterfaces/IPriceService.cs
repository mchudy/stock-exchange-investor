using System;
using StockExchange.Business.Extensions;
using StockExchange.Business.Models.Filters;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using StockExchange.Business.Models.Price;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface IPriceService
    {
        PagedList<PriceDto> Get(PagedFilterDefinition<PriceFilter> pagedFilterDefinition);

        object GetValues(FilterDefinition<PriceFilter> toPagedFilterDefinition, string fieldName);

        IList<CompanyPricesDto> GetPricesForCompanies(IList<int> companyIds);

        IList<Price> GetCurrentPrices(IList<int> companyIds);

        IList<Price> GetPrices(int companyId, DateTime endDate);

        Dictionary<int, decimal> GetPrices(IList<int> companyIds, DateTime date);
    }
}
