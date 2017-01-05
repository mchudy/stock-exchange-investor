using StockExchange.Business.Extensions;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Price;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface IPriceService
    {
        Task<PagedList<PriceDto>> GetPrices(PagedFilterDefinition<PriceFilter> pagedFilterDefinition);

        Task<IList<CompanyPricesDto>> GetPrices(IList<int> companyIds);
        Task<IList<Price>> GetPrices(int companyId, DateTime endDate);

        Task<object> GetFilterValues(FilterDefinition<PriceFilter> toPagedFilterDefinition, string fieldName);

        Task<IList<Price>> GetCurrentPrices(IList<int> companyIds);

        Task<IList<Price>> GetCurrentPrices(int days);

        Task<DateTime> GetMaxDate();

        Task<PagedList<MostActivePriceDto>> GetAdvancers(PagedFilterDefinition<TransactionFilter> message);

        Task<PagedList<MostActivePriceDto>> GetDecliners(PagedFilterDefinition<TransactionFilter> message);

        Task<PagedList<MostActivePriceDto>> GetMostActive(PagedFilterDefinition<TransactionFilter> message);
    }
}
