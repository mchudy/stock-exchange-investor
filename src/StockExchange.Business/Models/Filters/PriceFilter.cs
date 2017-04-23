using System;

namespace StockExchange.Business.Models.Filters
{
    /// <summary>
    /// A filter for price tables
    /// </summary>
    public sealed class PriceFilter : IFilter
    {
        /// <summary>
        /// Start date
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// End date
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Name of the company to filter
        /// </summary>
        public string CompanyName { get; set; }
    }
}
