using System.Collections.Generic;

namespace StockExchange.Business.Models.Price
{
    public class CompanyPricesDto
    {
        public DataAccess.Models.Company Company { get; set; }

        public IList<DataAccess.Models.Price> Prices { get; set; }
    }
}