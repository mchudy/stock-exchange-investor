using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Paging;
using StockExchange.Business.Models.Price;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchange.Business.ServiceInterfaces
{
    /// <summary>
    /// Provides methods for operating on stock prices
    /// </summary>
    public interface IPriceService
    {
        /// <summary>
        /// Returns the stock prices
        /// </summary>
        /// <param name="pagedFilterDefinition">The paging and filtering definition for prices</param>
        /// <returns>The paged and filtered list of prices</returns>
        Task<PagedList<PriceDto>> GetPrices(PagedFilterDefinition<PriceFilter> pagedFilterDefinition);

        /// <summary>
        /// Returns the stock prices
        /// </summary>
        /// <param name="companyIds">The company ids to include</param>
        /// <returns>The list of prices</returns>
        Task<IList<CompanyPricesDto>> GetPrices(IList<int> companyIds);

        /// <summary>
        /// Returns the stock prices
        /// </summary>
        /// <param name="companyId">The company ID</param>
        /// <param name="endDate">The date until which prices should be included</param>
        /// <returns>The list of prices</returns>
        Task<IList<Price>> GetPrices(int companyId, DateTime endDate);

        /// <summary>
        /// Returns a list of available property values for the <paramref name="fieldName"/>/>
        /// </summary>
        /// <param name="toPagedFilterDefinition">The paging and filtering definition for prices</param>
        /// <param name="fieldName">The property name</param>
        /// <returns>The list of available values</returns>
        Task<object> GetFilterValues(FilterDefinition<PriceFilter> toPagedFilterDefinition, string fieldName);

        /// <summary>
        /// Returns the current stock prices
        /// </summary>
        /// <param name="companyIds">Ids of companies to include</param>
        /// <returns>The list of current prices</returns>
        Task<IList<Price>> GetCurrentPrices(IList<int> companyIds);

        /// <summary>
        /// Returns the current stock prices
        /// </summary>
        /// <param name="days">Number of days back to include</param>
        /// <returns>The list of current prices</returns>
        Task<IList<Price>> GetCurrentPrices(int days);

        /// <summary>
        /// Returns the maximum date of price from the database
        /// </summary>
        /// <returns>The maximum date of price from the database</returns>
        Task<DateTime> GetMaxDate();

        /// <summary>
        /// Returns the companies with the highest gain recently and its current prices
        /// </summary>
        /// <param name="message">The paging and filtering definition for prices</param>
        /// <returns>The companies with the highest gain</returns>
        Task<PagedList<MostActivePriceDto>> GetAdvancers(PagedFilterDefinition<TransactionFilter> message);

        /// <summary>
        /// Returns the companies with the highest loss recently and its current prices
        /// </summary>
        /// <param name="message">The paging and filtering definition for prices</param>
        /// <returns>The companies with the highest loss</returns>
        Task<PagedList<MostActivePriceDto>> GetDecliners(PagedFilterDefinition<TransactionFilter> message);

        /// <summary>
        /// Returns the most active companies recently and its current prices
        /// </summary>
        /// <param name="message">The paging and filtering definition for prices</param>
        /// <returns>The most active companies</returns>
        Task<PagedList<MostActivePriceDto>> GetMostActive(PagedFilterDefinition<TransactionFilter> message);
    }
}
