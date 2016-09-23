namespace StockExchange.Business.Models
{
    public sealed class FilterDefinition<T> where T : IFilter
    {
        public string Search { get; set; }

        public T Filter { get; set; }
    }
}
