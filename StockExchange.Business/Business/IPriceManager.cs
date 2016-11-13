using StockExchange.Business.Extensions;
using StockExchange.Business.Models;
using System.Collections.Generic;

namespace StockExchange.Business.Business
{
    public interface IPriceManager
    {
        PagedList<PriceDto> Get(PagedFilterDefinition<PriceFilter> pagedFilterDefinition);

        IEnumerable<string> GetCompanyNames();
        IList<CompanyDto> GetAllCompanies();

        object GetValues(FilterDefinition<PriceFilter> toPagedFilterDefinition, string fieldName);

        IList<CompanyPricesDto> GetPricesForCompanies(IList<int> companyIds);
    }
}
