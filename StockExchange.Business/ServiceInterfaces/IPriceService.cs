using StockExchange.Business.Extensions;
using StockExchange.Business.Models;
using StockExchange.Business.Models.Filters;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface IPriceService
    {
        PagedList<PriceDto> Get(PagedFilterDefinition<PriceFilter> pagedFilterDefinition);

        IEnumerable<string> GetCompanyNames();

        IList<CompanyDto> GetAllCompanies();

        object GetValues(FilterDefinition<PriceFilter> toPagedFilterDefinition, string fieldName);

        IList<CompanyPricesDto> GetPricesForCompanies(IList<int> companyIds);

        IList<Price> GetCurrentPrices(IList<int> companyIds);
    }
}
