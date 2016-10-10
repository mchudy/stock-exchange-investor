using System.Collections.Generic;
using StockExchange.Business.Models;

namespace StockExchange.Web.Models
{
    public class DataTableMessage
    {
        public class DataTableMessageColumn
        {
            public string Data { get; set; }
            public string Name { get; set; }
            public bool Searchable { get; set; }
            public bool Orderable { get; set; }
            public DataTableMessageSearch Search { get; set; }
        }

        public class DataTableMessageOrder
        {
            public int Column { get; set; }
            public string Dir { get; set; }
            public bool Desc => Dir == "desc";
        }

        public class DataTableMessageSearch
        {
            public string Value { get; set; }
            public bool Regex { get; set; }
        }

        public int Draw { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public DataTableMessageSearch Search { get; set; }

        public List<DataTableMessageColumn> Columns { get; set; }

        public List<DataTableMessageOrder> Order { get; set; }
    }

    public class DataTableMessage<T> : DataTableMessage where T : IFilter
    {
        public T Filter { get; set; }
    }
}



