using StockExchange.DataAccess.Models;
using System.Collections.Generic;

namespace StockExchange.Business.Models
{
    public class CompanyPricesDto
    {
        public Company Company { get; set; }

        public IList<Price> Prices { get; set; }
    }
}