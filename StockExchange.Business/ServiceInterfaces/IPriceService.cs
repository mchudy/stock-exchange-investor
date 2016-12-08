using System.Collections.Generic;
using StockExchange.Business.Extensions;
using StockExchange.Business.Models;
using StockExchange.Business.Models.Filters;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface IPriceService
    {
        PagedList<PriceDto> Get(PagedFilterDefinition<PriceFilter> pagedFilterDefinition);

        IEnumerable<string> GetCompanyNames();

        IList<CompanyDto> GetAllCompanies();

        object GetValues(FilterDefinition<PriceFilter> toPagedFilterDefinition, string fieldName);

        IList<CompanyPricesDto> GetPricesForCompanies(IList<int> companyIds);
    }
}
