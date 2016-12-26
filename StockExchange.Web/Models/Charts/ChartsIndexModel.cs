using StockExchange.Business.Models;
using System.Collections.Generic;
using StockExchange.Business.Models.Company;

namespace StockExchange.Web.Models.Charts
{
    public class ChartsIndexModel
    {
        public IList<CompanyDto> Companies { get; set; }
        public IList<IndicatorViewModel> Indicators { get; set; }
    }
}