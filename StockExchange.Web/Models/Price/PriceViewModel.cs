using System.Collections.Generic;
using StockExchange.Business.Models.Price;

namespace StockExchange.Web.Models.Price
{
    public sealed class PriceViewModel
    {
        public IEnumerable<PriceDto> Prices { get; set; }

        public IEnumerable<string> CompanyNames { get; set; }
    }
}
