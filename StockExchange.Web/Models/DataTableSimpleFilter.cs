using StockExchange.Business.Models;

namespace StockExchange.Web.Models
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