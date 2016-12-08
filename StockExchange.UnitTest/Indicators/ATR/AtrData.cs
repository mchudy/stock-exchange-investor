using StockExchange.Business.Models;
using StockExchange.DataAccess.Models;
using StockExchange.UnitTest.Helpers;
using System.Collections.Generic;
using StockExchange.Business.Models.Indicators;

namespace StockExchange.UnitTest.Indicators.ATR
{
    internal class AtrData
    {
        public const int DataPrecision = 6;

        internal static IList<Price> HistoricalData => DataHelper.ConvertToPrices(new[,]
        {
            { 67.27m,   65.75m,   65.98m},
            { 65.7m,    65.04m,   65.11m} ,
            { 65.05m,   64.26m,   64.97m},
            { 65.16m,   64.09m,   64.29m},
            { 62.73m,   61.85m,   62.44m},
            { 62.02m,   61.29m,   61.47m},
            { 62.75m,   61.55m,   61.59m},
            { 64.78m,   62.22m,   64.64m},
            { 64.5m,    63.03m,   63.28m},
            { 63.7m,    62.7m,    63.59m} ,
            { 64.45m,   63.26m,   63.61m},
            { 64.56m,   63.81m,   64.52m},
            { 64.84m,   63.66m,   63.91m},
            { 65.3m,    64.5m,    65.22m} ,
            { 65.36m,   64.46m,   65.06m},
            { 64.54m,   63.56m,   63.65m},
            { 64.03m,   63.33m,   63.73m},
            { 63.4m,    62.8m,    62.83m} ,
            { 63.75m,   62.96m,   63.6m} ,
            { 63.64m,   62.51m,   63.51m},
            { 64.03m,   63.53m,   63.76m},
            { 63.77m,   63.01m,   63.65m},
            { 63.95m,   63.58m,   63.79m},
            { 63.47m,   62.92m,   63.25m},
            { 63.96m,   63.21m,   63.48m},
            { 63.63m,   62.55m,    63.5m},
            { 63.25m,   62.82m,    62.9m},
            { 62.34m,   62.05m,   62.18m},
            { 62.86m,   61.94m,   62.81m},
            { 63.06m,   62.44m,   62.83m}
        });

        internal static IList<IndicatorValue> Term14Results => DataHelper.ConvertToIndicatorValues(new[]
        {
            1.411429m,
            1.374898m,
            1.383834m,
            1.334989m,
            1.306061m,
            1.278485m,
            1.267879m,
            1.214459m,
            1.181998m,
            1.123998m,
            1.105855m,
            1.080437m,
            1.080406m,
            1.051805m,
            1.037391m,
            1.029006m,
            0.999791m,
        }, 13);

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
