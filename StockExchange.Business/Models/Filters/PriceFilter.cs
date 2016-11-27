using System;

namespace StockExchange.Business.Models.Filters
{
    public sealed class PriceFilter : IFilter
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string CompanyName { get; set; }
    }
}
