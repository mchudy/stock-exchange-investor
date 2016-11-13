using StockExchange.Business.Models;
using System.Collections.Generic;

namespace StockExchange.Web.Models.Charts
{
    public class ChartsIndexModel
    {
        public IList<CompanyDto> Companies { get; set; }
    }
}