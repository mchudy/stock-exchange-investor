﻿using System;
using System.Collections.Generic;
using StockExchange.Business.Indicators;
using StockExchange.DataAccess.Models;

namespace StockExchange.UnitTest.Indicators
{
    /// <summary>
    /// This data are based on example:
    /// http://investexcel.net/how-to-calculate-macd-in-excel/
    /// </summary>
    internal class MacdData
    {
        public static readonly DateTime StartDate = new DateTime(2015, 11,1);
        public static readonly IList<Price> HistorcalData;

        static MacdData()
        {
            HistorcalData = GetHistoricalData();
        }

        internal static IList<Price> GetHistoricalData()
        {
            var tab = new[]
            {
                459.99m,
                448.85m,
                446.06m,
                450.81m,
                442.8m,
                448.97m,
                444.57m,
                441.4m,
                430.47m,
                420.05m,
                431.14m,
                425.66m,
                430.58m,
                431.72m,
                437.87m,
                428.43m,
                428.35m,
                432.5m,
                443.66m,
                455.72m,
                454.49m,
                452.08m,
                452.73m,
                461.91m,
                463.58m,
                461.14m,
                452.08m,
                442.66m,
                428.91m,
                429.79m,
                431.99m,
                427.72m,
                423.2m,
                426.21m,
                426.98m,
                435.69m,
                434.33m,
                429.8m,
                419.85m,
                426.24m,
                402.8m,
                392.05m,
                390.53m,
                398.67m,
                406.13m,
                405.46m,
                408.38m,
                417.2m,
                430.12m,
                442.78m,
                439.29m,
                445.52m,
                449.98m,
                460.71m,
                458.66m,
                463.84m,
                456.77m,
                452.97m,
                454.74m,
                443.86m,
                428.85m,
                434.58m,
                433.26m,
                442.93m,
                439.66m,
                441.35m
            };
            IList<Price> prices = new List<Price>();
            for (int i = 0; i < tab.Length; i++)
            {
                prices.Add(new Price()
                {
                    ClosePrice = tab[i],
                    Date = StartDate.AddDays(i)
                });
            }
            return prices;
        }

        internal static IList<IndicatorValue> Get12DaysEma()
        {
            var tab = new[]
            {
                440.8975m,
                439.3101923m,
                438.1424704m,
                438.1005519m,
                436.6127747m,
                435.3415786m,
                434.9044126m,
                436.2514261m,
                439.2465913m,
                441.5917311m,
                443.2053109m,
                444.6706477m,
                447.3228558m,
                449.8239549m,
                451.5648849m,
                451.6441334m,
                450.261959m,
                446.9770422m,
                444.3328819m,
                442.433977m,
                440.1702882m,
                437.5594746m,
                435.8134016m,
                434.4544168m,
                434.6445065m,
                434.5961209m,
                433.8582561m,
                431.7031398m,
                430.8626568m,
                426.5453249m,
                421.2383519m,
                416.51399m,
                413.7687608m,
                412.5935668m,
                411.496095m,
                411.0166958m,
                411.9679734m,
                414.7605928m,
                419.0712709m,
                422.1818446m,
                425.77233m,
                429.4965869m,
                434.2986505m,
                438.0465504m,
                442.0147734m,
                444.2848083m,
                445.6209916m,
                447.023916m,
                446.5371597m,
                443.8160582m,
                442.3951262m,
                440.9897221m,
                441.2882264m,
                441.0377301m,
                441.0857716m
            };
            var prices = new List<IndicatorValue>();
            for (int i = 0; i < tab.Length; i++)
            {
                prices.Add(new IndicatorValue()
                {
                    Value = tab[i],
                    Date = StartDate.AddDays(i+12)
                });
            }
            return prices;
        }

        internal static IList<IndicatorValue> Get26DaysEma()
        {
            var tab = new[]
            {
                443.2896154m,
                443.940755m,
                443.8458842m,
                442.7395225m,
                441.7802986m,
                441.0550913m,
                440.0673067m,
                438.8178766m,
                437.8839598m,
                437.0762591m,
                436.9735732m,
                436.777753m,
                436.2608824m,
                435.0452615m,
                434.3930199m,
                432.0527962m,
                429.0896261m,
                426.2333575m,
                424.1916273m,
                422.853729m,
                421.5653046m,
                420.5886154m,
                420.3376068m,
                421.0622286m,
                422.6709524m,
                423.9019929m,
                425.5033268m,
                427.3164137m,
                429.7900127m,
                431.9285303m,
                434.2923428m,
                435.9573545m,
                437.2175504m,
                438.5155097m,
                438.9113978m,
                438.1661091m,
                437.9004714m,
                437.5567328m,
                437.9547526m,
                438.0810672m,
                438.3232104m,
            };
            var prices = new List<IndicatorValue>();
            for (int i = 0; i < tab.Length; i++)
            {
                prices.Add(new IndicatorValue()
                {
                    Value = tab[i],
                    Date = StartDate.AddDays(i + 26)
                });
            }
            return prices;
        }

        internal static IList<IndicatorValue> Get9DaysSignal()
        {
            decimal[] tab = new[]
            {
                3.037525869m,
                1.905652229m,
                1.058708435m,
                0.410640325m,
                -0.152012994m,
                -0.790034732m,
                -1.338100413m,
                -2.17197458m,
                -3.30783451m,
                -4.590141099m,
                -5.756686181m,
                -6.657381376m,
                -7.339747023m,
                -7.786181541m,
                -7.902871931m,
                -7.58262469m,
                -6.786036054m,
                -5.772858515m,
                -4.564486166m,
                -3.215554283m,
                -1.670715865m,
                -0.112968661m,
                1.45411119m,
                2.828779714m,
                3.943712008m,
                4.856650871m,
                5.410473066m,
                5.458368269m,
                5.265625568m,
                4.899098327m,
                4.585973432m,
                4.260111317m,
                3.960601297m
            };
            var prices = new List<IndicatorValue>();
            for (int i = 0; i < tab.Length; i++)
            {
                prices.Add(new IndicatorValue()
                {
                    Value = tab[i],
                    Date = StartDate.AddDays(i + 33)
                });
            }
            return prices;
        }
    }
}
