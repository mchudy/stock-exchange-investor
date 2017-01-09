using System.Collections.Generic;

namespace StockExchange.Business.Models.Indicators
{
    /// <summary>
    /// Contains indicator values computed for the company
    /// </summary>
    public class CompanyIndicatorValues
    {
        /// <summary>
        /// The company
        /// </summary>
        public DataAccess.Models.Company Company { get; set; }

        /// <summary>
        /// The computed indicator values
        /// </summary>
        public IList<IndicatorValue> IndicatorValues { get; set; } = new List<IndicatorValue>();
    }
}
