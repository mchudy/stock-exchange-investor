using System.Collections.Generic;
using StockExchange.Common.LinqUtils;

namespace StockExchange.Business.Models.Filters
{
    public sealed class PagedFilterDefinition<T> where T : IFilter
    {
        public int Start { get; set; }

        public int Length { get; set; }

        public T Filter { get; set; }

        public string Search { get; set; }

        public List<OrderBy> OrderBys { get; set; }

        public List<SearchBy> Searches { get; set; }
    }
}
