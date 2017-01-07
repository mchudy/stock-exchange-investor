using StockExchange.Business.Models.Filters;

namespace StockExchange.Web.Models.DataTables
{
    /// <summary>
    /// Simplified DataTable message
    /// </summary>
    public class DataTableSimpleMessage
    {
        /// <summary>
        /// Search object for the message
        /// </summary>
        public DataTableMessageSearch Search { get; set; }
    }

    /// <summary>
    /// Simplified DataTable message with filtering
    /// </summary>
    /// <typeparam name="T">Object to filter</typeparam>
    public class DataTableSimpleMessage<T> : DataTableSimpleMessage where T : IFilter
    {
        /// <summary>
        /// A filter object for filtering the data
        /// </summary>
        public T Filter { get; set; }
    }
}