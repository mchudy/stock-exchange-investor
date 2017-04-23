namespace StockExchange.DataAccess.Cache
{
    /// <summary>
    /// Settings for connecting with Redis instances
    /// </summary>
    public interface IRedisSettings
    {
        /// <summary>
        /// The connection string to the instance
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// The database number (0 by default)
        /// </summary>
        int DatabaseNumber { get; }
    }
}