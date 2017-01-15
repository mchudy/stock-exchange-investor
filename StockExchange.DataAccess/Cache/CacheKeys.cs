using System.Collections.Generic;
using System.Linq;

namespace StockExchange.DataAccess.Cache
{
    /// <summary>
    /// Contains cache keys
    /// </summary>
    public static class CacheKeys
    {
        public const string MaxDate = "MaxDate";
        public const string TwoMaxDates = "TwoMaxDates";
        public const string PricesForDate = "Prices";
        public const string CurrentSignalsCount = "CurrentSignalsCount";

        public static string CurrentPrices(IList<int> companyIds) => 
            "CurrentPrices" + "_" + string.Join(",", companyIds.OrderBy(x => x));

        public static string OwnedStocks(int userId) => $"OwnedStocks_{userId}";

        public static string TransactionsCount(int userId) => $"TransactionsCount_{userId}";

        public static string CurrentSignals(int start, int length) => 
            $"CurrentSignals_{start}_{length}";

    }
}
