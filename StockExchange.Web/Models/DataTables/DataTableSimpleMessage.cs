using StockExchange.Business.Models;
using StockExchange.Business.Models.Filters;

namespace StockExchange.Web.Models.DataTables
{
    public class DataTableSimpleMessage
    {
        public class DataTableMessageSearch
        {
            public string Value { get; set; }

            public bool Regex { get; set; }
        }

        public DataTableMessageSearch Search { get; set; }
    }

    public class DataTableSimpleMessage<T> : DataTableSimpleMessage where T : IFilter
    {
        public T Filter { get; set; }
    }
}