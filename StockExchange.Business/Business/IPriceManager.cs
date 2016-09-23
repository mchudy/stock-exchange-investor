using System.Collections.Generic;
using StockExchange.Business.Models;
using StockExchange.Common;

namespace StockExchange.Business.Business
{
    public interface IPriceManager
    {
        PagedList<PriceDto> Get(PagedFilterDefinition<PriceFilter> pagedFilterDefinition);

        IEnumerable<string> GetCompanyNames();

        object GetValues(FilterDefinition<PriceFilter> toPagedFilterDefinition, string fieldName);
    }
}
