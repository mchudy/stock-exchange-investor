using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using StockExchange.UnitTest.TestHelpers;
using System.Collections.Generic;

namespace StockExchange.UnitTest.Indicators.SMA
{
    internal class SmaData
    {
        public const int DataPrecision = 6;

        internal static IList<Price> HistoricalData => DataHelper.ConvertToPrices(new[,]
        {
            { 1m, 1m, 1m},
            { 2m, 2m, 2m} ,
            { 3m, 3m, 3m},
            { 4m, 4m, 4m},
            { 5m, 5m, 5m},
            { 6m, 6m, 6m},
            { 7m, 7m, 7m}
        });

        internal static IList<IndicatorValue> Results => DataHelper.ConvertToIndicatorValues(new[]
        {
            3m,
            4m,
            5m
        }, 4);
    }
}
