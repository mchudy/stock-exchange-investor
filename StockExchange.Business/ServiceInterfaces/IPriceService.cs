using StockExchange.Business.Extensions;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Price;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface IPriceService
    {
        PagedList<PriceDto> Get(PagedFilterDefinition<PriceFilter> pagedFilterDefinition);

        object GetValues(FilterDefinition<PriceFilter> toPagedFilterDefinition, string fieldName);

        IList<CompanyPricesDto> GetPricesForCompanies(IList<int> companyIds);

        IList<Price> GetCurrentPrices(IList<int> companyIds);

        IList<Price> GetPrices(int companyId, DateTime endDate);

        IList<Price> GetLastPricesForAllCompanies();

        DateTime GetMaxDate();

        PagedList<MostActivePriceDto> GetAdvancers(PagedFilterDefinition<TransactionFilter> message);

        PagedList<MostActivePriceDto> GetDecliners(PagedFilterDefinition<TransactionFilter> message);

        PagedList<MostActivePriceDto> GetMostAactive(PagedFilterDefinition<TransactionFilter> message);
    }
}
