namespace StockExchange.Common
{
    /// <summary>
    /// A generic factory class
    /// </summary>
    /// <typeparam name="T">Type of objects created</typeparam>
    public sealed class Factory<T> : IFactory<T> where T : new()
    {
        /// <inheritdoc />
        public T CreateInstance()
        {
            return new T();
        }
    }
}
