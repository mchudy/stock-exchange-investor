using StockExchange.DataAccess.Models;
using System.Collections.Generic;

namespace StockExchange.Business.Models.Indicators
{
    public class CompanyIndicatorValues
    {
        public Company Company { get; set; }
        public IList<IndicatorValue> IndicatorValues { get; set; } = new List<IndicatorValue>();
    }
}
