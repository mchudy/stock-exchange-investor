using System.Collections.Generic;
using StockExchange.Business.Models;
using StockExchange.Business.Models.Price;

namespace StockExchange.Web.Models
{
    public sealed class PriceViewModel
    {
        public IEnumerable<PriceDto> Prices { get; set; }

        public IEnumerable<string> CompanyNames { get; set; }
    }
}
