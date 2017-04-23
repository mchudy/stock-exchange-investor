using StockExchange.Business.Models.Filters;
using System.Collections.Generic;

namespace StockExchange.Web.Models.DataTables
{
    /// <summary>
    /// Message for paginating with DataTable
    /// </summary>
    public class DataTableMessage
    {
        /// <summary>
        /// Draw
        /// </summary>
        public int Draw { get; set; }

        /// <summary>
        /// Starting index
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Number of items to load
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Search parameters
        /// </summary>
        public DataTableMessageSearch Search { get; set; }

        /// <summary>
        /// Columns to include
        /// </summary>
        public List<DataTableMessageColumn> Columns { get; set; }

        /// <summary>
        /// Ordering to perform
        /// </summary>
        public List<DataTableMessageOrder> Order { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataTableMessage<T> : DataTableMessage where T : IFilter
    {
        /// <summary>
        /// Filter object for filtering the data
        /// </summary>
        public T Filter { get; set; }
    }

    /// <summary>
    /// A single data column
    /// </summary>
    public class DataTableMessageColumn
    {
        /// <summary>
        /// The column data
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// The column name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whether the column is searchable
        /// </summary>
        public bool Searchable { get; set; }

        /// <summary>
        /// Whether the column is orderable
        /// </summary>
        public bool Orderable { get; set; }

        /// <summary>
        /// Search object for the column
        /// </summary>
        public DataTableMessageSearch Search { get; set; }
    }

    /// <summary>
    /// Message for DataTable ordering 
    /// </summary>
    public class DataTableMessageOrder
    {
        /// <summary>
        /// Column by which the data is to be ordered
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Sorting direction
        /// </summary>
        public string Dir { get; set; }

        /// <summary>
        /// Whether the direction is descending
        /// </summary>
        public bool Desc => Dir == "desc";
    }

    /// <summary>
    /// Message for DataTable searching 
    /// </summary>
    public class DataTableMessageSearch
    {
        /// <summary>
        /// Value to search
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Regular expression to match
        /// </summary>
        public bool Regex { get; set; }
    }
}



