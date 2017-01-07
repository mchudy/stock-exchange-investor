using StockExchange.Business.Models.Price;
using System.Collections.Generic;

namespace StockExchange.Web.Models.Price
{
    /// <summary>
    /// View model for prices of multiple companies
    /// </summary>
    public sealed class PriceViewModel
    {
        /// <summary>
        /// Prices
        /// </summary>
        public IEnumerable<PriceDto> Prices { get; set; }

        /// <summary>
        /// Company names
        /// </summary>
        public IEnumerable<string> CompanyNames { get; set; }
    }
}
