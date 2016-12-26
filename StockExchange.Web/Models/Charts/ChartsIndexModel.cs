using System.Collections.Generic;
using StockExchange.Business.Models.Company;
using StockExchange.Web.Models.Indicator;

namespace StockExchange.Web.Models.Charts
{
    public class ChartsIndexModel
    {
        public IList<CompanyDto> Companies { get; set; }

        public IList<IndicatorViewModel> Indicators { get; set; }
    }
}