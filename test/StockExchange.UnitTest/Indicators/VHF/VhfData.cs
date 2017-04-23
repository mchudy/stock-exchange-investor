using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using StockExchange.UnitTest.TestHelpers;
using System.Collections.Generic;

namespace StockExchange.UnitTest.Indicators.VHF
{
    internal class VhfData
    {
        public const int DataPrecision = 4;

        internal static IList<Price> HistoricalData => DataHelper.ConvertToPrices(new[,]
        {
            { 20m, 20m, 20m},
            { 21m, 21m, 21m} ,
            { 22m, 22m, 22m},
            { 23m, 23m, 23m},
            { 22m, 22m, 22m},
            { 21m, 21m, 21m}
        });

        internal static IList<IndicatorValue> Results => DataHelper.ConvertToIndicatorValues(new[]
        {
            .6667m,
            .3333m,
            .6667m
        }, 2);
    }
}
