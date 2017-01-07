using StockExchange.Common.LinqUtils;
using System.Collections.Generic;

namespace StockExchange.Business.Models.Filters
{
    /// <summary>
    /// The paged filter definition
    /// </summary>
    /// <typeparam name="T">Type of objects to filter and page</typeparam>
    public sealed class PagedFilterDefinition<T> where T : IFilter
    {
        /// <summary>
        /// The start from which items shoul be taken
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Number of items
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// The filter
        /// </summary>
        public T Filter { get; set; }

        /// <summary>
        /// The search query
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// The objects representing the ordering
        /// </summary>
        public List<OrderBy> OrderBys { get; set; }

        /// <summary>
        /// The objects representing the search filters
        /// </summary>
        public List<SearchBy> Searches { get; set; }
    }
}
