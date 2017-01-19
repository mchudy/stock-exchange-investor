using StockExchange.Business.Models.Company;
using StockExchange.Web.Models.Indicator;
using System.Collections.Generic;

namespace StockExchange.Web.Models.Charts
{
    /// <summary>
    /// View model for the charts view
    /// </summary>
    public class ChartsIndexModel
    {
        /// <summary>
        /// List of all companies
        /// </summary>
        public IList<CompanyDto> Companies { get; set; }

        /// <summary>
        /// List of all available indicators
        /// </summary>
        public IList<EditIndicatorViewModel> Indicators { get; set; }

        /// <summary>
        /// List of all company groups
        /// </summary>
        public IList<CompanyGroupDto> CompanyGroups { get; set; }
    }
}