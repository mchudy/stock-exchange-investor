using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using StockExchange.UnitTest.TestHelpers;
using System.Collections.Generic;

namespace StockExchange.UnitTest.Indicators.ROC
{
    internal class RocData
    {
        public const int DataPrecision = 2;

        internal static IList<Price> HistoricalData => DataHelper.ConvertToPrices(new[]
        {
            11045.27m,
            11167.32m,
            11008.61m,
            11151.83m,
            10926.77m,
            10868.12m,
            10520.32m,
            10380.43m,
            10785.14m,
            10748.26m,
            10896.91m,
            10782.95m,
            10620.16m,
            10625.83m,
            10510.95m,
            10444.37m,
            10068.01m,
            10193.39m,
            10066.57m,
            10043.75m,
        });

        internal static IList<IndicatorValue> Term12Results => DataHelper.ConvertToIndicatorValues(new[]
        {
           -3.85m,
           -4.85m,
           -4.52m,
           -6.34m,
           -7.86m,
           -6.21m,
           -4.31m,
           -3.24m
        }, 12);
    }
}
