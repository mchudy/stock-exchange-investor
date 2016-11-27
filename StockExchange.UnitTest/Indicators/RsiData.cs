using StockExchange.Business.Models;
using StockExchange.DataAccess.Models;
using StockExchange.UnitTest.Helpers;
using System.Collections.Generic;

namespace StockExchange.UnitTest.Indicators
{
    internal class RsiData
    {
        public const int DataPrecision = 6;

        internal static IList<Price> HistoricalData => DataHelper.ConvertToPrices(new[]
        {
            45.15m,
            46.26m,
            46.5m,
            46.23m,
            46.08m,
            46.03m,
            46.83m,
            47.69m,
            47.54m,
            49.25m,
            49.23m,
            48.2m,
            47.57m,
            47.61m,
            48.08m,
            47.21m,
            46.76m,
            46.68m,
            46.21m,
            47.47m,
            47.98m,
            47.13m,
            46.58m,
            46.03m,
            46.54m,
            46.79m
        });

        internal static IList<IndicatorValue> Term14Results => DataHelper.ConvertToIndicatorValues(new[]
        {
            69.455511m,
            61.769783m,
            58.183410m,
            57.543764m,
            53.801555m,
            61.104336m,
            63.611644m,
            57.014919m,
            53.172478m,
            49.574475m,
            52.766160m,
            54.293386m,
        }, 14);

        internal static IList<IndicatorValue> Term20Results => DataHelper.ConvertToIndicatorValues(new[]
        {
            62.667860m,
            58.020329m,
            55.230506m,
            52.569728m,
            54.699891m,
            55.725904m,
        }, 20);
    }
}
