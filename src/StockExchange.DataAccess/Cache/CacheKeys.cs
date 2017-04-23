using System.Collections.Generic;
using System.Linq;

namespace StockExchange.DataAccess.Cache
{
    /// <summary>
    /// Contains cache keys
    /// </summary>
    public static class CacheKeys
    {
        /// <summary>
        /// Cache key for max date
        /// </summary>
        public const string MaxDate = "MaxDate";

        /// <summary>
        /// Cache key for latest two days from which there are prices in the
        /// database
        /// </summary>
        public const string TwoMaxDates = "TwoMaxDates";

        /// <summary>
        /// Cache key number of generated signals
        /// </summary>
        public const string CurrentSignalsCount = "CurrentSignalsCount";

        /// <summary>
        /// Cache key for of all generated signals
        /// </summary>
        public const string AllCurrentSignals = "AllCurrentSignals";

        /// <summary>
        /// Constructs a cache key for current prices for the given companies
        /// </summary>
        /// <param name="companyIds">Companies ids</param>
        /// <returns>Constructed cache key</returns>
        public static string CurrentPrices(IList<int> companyIds) => 
            "CurrentPrices" + "_" + string.Join(",", companyIds.OrderBy(x => x));

        /// <summary>
        /// Constructs a cache key for stocks owned by the given user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Constructed cache key</returns>
        public static string OwnedStocks(int userId) => $"OwnedStocks_{userId}";

        /// <summary>
        /// Constructs a cache key for the number of transactions concluded by the given user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Constructed cache key</returns>
        public static string TransactionsCount(int userId) => $"TransactionsCount_{userId}";
    }
}
