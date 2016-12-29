using StockExchange.Business.Models.Company;
using StockExchange.Web.Models.Indicator;
using System.Collections.Generic;

namespace StockExchange.Web.Models.Charts
{
    public class ChartsIndexModel
    {
        public IList<CompanyDto> Companies { get; set; }

        public IList<EditIndicatorViewModel> Indicators { get; set; }
    }
}