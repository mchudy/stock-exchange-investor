using System.Collections.Generic;
using StockExchange.Business.Models;

namespace StockExchange.Web.Models
{
    public class DataTableFilter
    {
        public sealed class DataTableFilterColumn
        {
            public string Data { get; set; }

            public string Name { get; set; }

            public bool Searchable { get; set; }

            public bool Orderable { get; set; }

            public DataTableFilterSearch Search { get; set; }
        }

        public sealed class DataTableFilterOrder
        {
            public int Column { get; set; }

            public string Dir { get; set; }

            public bool Desc => Dir == "desc";
        }

        public sealed class DataTableFilterSearch
        {
            public string Value { get; set; }

            public bool Regex { get; set; }
        }

        public int Draw { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public DataTableFilterSearch Search { get; set; }

        public List<DataTableFilterColumn> Columns { get; set; }

        public List<DataTableFilterOrder> Order { get; set; }
    }

    public sealed class DataTableFilter<T> : DataTableFilter where T : IFilter
    {
        public T Filter { get; set; }
    }
}



