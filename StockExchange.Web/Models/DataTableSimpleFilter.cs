using StockExchange.Business.Models;

namespace StockExchange.Web.Models
{
    public class DataTableSimpleFilter
    {
        public sealed class DataTableFilterSearch
        {
            public string Value { get; set; }

            public bool Regex { get; set; }
        }

        public DataTableFilterSearch Search { get; set; }
    }

    public sealed class DataTableSimpleFilter<T> : DataTableSimpleFilter where T : IFilter
    {
        public T Filter { get; set; }
    }
}