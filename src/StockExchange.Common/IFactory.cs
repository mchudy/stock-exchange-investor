namespace StockExchange.Common
{
    /// <summary>
    /// An abstract interface for factories
    /// </summary>
    /// <typeparam name="T">Type of objects created</typeparam>
    public interface IFactory<out T>
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        T CreateInstance();
    }
}
