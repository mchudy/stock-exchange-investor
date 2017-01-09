using System.Collections.Generic;

namespace StockExchange.Business.Models.Price
{
    /// <summary>
    /// Contains the prices for a company
    /// </summary>
    public class CompanyPricesDto
    {
        /// <summary>
        /// The company
        /// </summary>
        public DataAccess.Models.Company Company { get; set; }

        /// <summary>
        /// The prices for the company
        /// </summary>
        public IList<DataAccess.Models.Price> Prices { get; set; }
    }
}