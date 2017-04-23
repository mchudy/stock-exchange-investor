using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using StockExchange.UnitTest.TestHelpers;
using System.Collections.Generic;

namespace StockExchange.UnitTest.Indicators.VROC
{
    internal class VrocData
    {
        public const int DataPrecision = 2;

        internal static IList<Price> HistoricalData => DataHelper.ConvertToPricesVolume(new[]
        {
            11045,
            11167,
            11008,
            11151,
            10926,
            10868,
            10520,
            10380,
            10785,
            10748,
            10896,
            10782,
            10620,
            10625,
            11045,
            11167,
            11008,
            11151,
            10926,
            10868
        });

        internal static IList<IndicatorValue> Results => DataHelper.ConvertToIndicatorValues(new[]
        {
           -0m,
           0m,
           0m,
           0m,
           0m,
           0m
        }, 14);
    }
}
