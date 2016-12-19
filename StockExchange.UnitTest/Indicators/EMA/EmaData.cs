using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using StockExchange.UnitTest.TestHelpers;
using System.Collections.Generic;

namespace StockExchange.UnitTest.Indicators.EMA
{
    internal class EmaData
    {
        public const int DataPrecision = 6;

        internal static IList<Price> HistoricalData => DataHelper.ConvertToPrices(new[,]
        {
            { 19m, 19m, 19m},
            { 20m, 20m, 20m} ,
            { 50m, 50m, 50m},
            { 22m, 22m, 22m},
            { 23m, 23m, 23m}
        });

        internal static IList<IndicatorValue> Results => DataHelper.ConvertToIndicatorValues(new[]
        {
            //17m,
            //18m,
            //28.67m,
            //26.44m,
            25.3m
        }, 4);
    }
}
