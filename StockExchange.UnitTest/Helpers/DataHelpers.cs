using StockExchange.Business.Models;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;

namespace StockExchange.UnitTest.Helpers
{
    public class DataHelper
    {
        public static readonly DateTime StartDate = new DateTime(2015, 11, 1);

        public static IList<IndicatorValue> ConvertToIndicatorValues(decimal[] tab, int offset = 0)
        {
            var prices = new List<IndicatorValue>();
            for (int i = 0; i < tab.Length; i++)
            {
                prices.Add(new IndicatorValue
                {
                    Value = tab[i],
                    Date = StartDate.AddDays(i + offset)
                });
            }
            return prices;
        }

        public static IList<Price> ConvertToPrices(decimal[] tab, int offset = 0)
        {
            var prices = new List<Price>();
            for (int i = 0; i < tab.Length; i++)
            {
                prices.Add(new Price
                {
                    ClosePrice = tab[i],
                    Date = StartDate.AddDays(i + offset)
                });
            }
            return prices;
        }
    }
}
