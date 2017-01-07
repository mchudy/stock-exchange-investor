namespace StockExchange.Business.Models.Filters
{
    /// <summary>
    /// The filter definition
    /// </summary>
    /// <typeparam name="T">Type of object to filter</typeparam>
    public sealed class FilterDefinition<T> where T : IFilter
    {
        /// <summary>
        /// The search query
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// The filter
        /// </summary>
        public T Filter { get; set; }
    }
}
