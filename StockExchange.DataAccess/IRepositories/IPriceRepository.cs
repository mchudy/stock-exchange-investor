using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.IRepositories
{
    /// <summary>
    /// Repository for <see cref="Price"/> entities
    /// </summary>
    public interface IPriceRepository : IRepository<Price>
    {
        /// <summary>
        /// Returns prices from the last <see paramref="days"/> days
        /// </summary>
        /// <param name="days">Number of days to include</param>
        /// <returns>List of prices</returns>
        Task<IList<Price>> GetCurrentPrices(int days);

        /// <summary>
        /// Returns latest prices for the given companies
        /// </summary>
        /// <param name="companyIds">Companies for which to load prices</param>
        /// <returns>List of prices</returns>
        Task<IList<Price>> GetCurrentPrices(IList<int> companyIds);

        /// <summary>
        /// Returns prices
        /// </summary>
        /// <param name="companyIds">Company ids for which to get the prices</param>
        /// <returns>Dictionary which keys are companies and values are lists of prices</returns>
        Task<Dictionary<Company, List<Price>>> GetPrices(IList<int> companyIds);

        /// <summary>
        /// Returns prices for the given company until the given date
        /// </summary>
        /// <param name="companyId">Company id</param>
        /// <param name="endDate">Date until to include the prices</param>
        /// <returns>List of prices</returns>
        Task<IList<Price>> GetPrices(int companyId, DateTime endDate);

        /// <summary>
        /// Returns the latest date from which prices are available
        /// </summary>
        /// <returns>The date</returns>
        Task<DateTime> GetMaxDate();

        /// <summary>
        /// Returns two latest dates from which prices are available
        /// </summary>
        /// <returns>Two latest dates from which prices are available</returns>
        Task<IList<DateTime>> GetTwoMaxDates();

        /// <summary>
        /// Returns prices for the given dates
        /// </summary>
        /// <param name="dates">Dates for which to get prices</param>
        /// <returns>List of prices</returns>
        Task<List<Price>> GetPricesForDates(IList<DateTime> dates);

    }
}