namespace StockExchange.Common
{
    public sealed class Factory<T> : IFactory<T> where T : new()
    {
        public T CreateInstance()
        {
            return new T();
        }
    }
}
