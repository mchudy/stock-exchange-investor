using System;
using System.Collections.Generic;
using System.Linq;
using StockExchange.DataAccess.Models;

namespace StockExchange.Business.Indicators
{
    public static class MovingAverageHelper
    {
        //Checks whether prices are sorted by date and there are no holes between prices.
        private static bool CheckDates(IList<DateTime> dates)
        {
            for (int i = 1; i < dates.Count; i++)
                if (dates[i].Date - dates[i - 1].Date > TimeSpan.FromDays(1))
                    return false;
            return true;
        }

        public static IndicatorValue SimpleMovingAverage(IList<Price> prices)
        {
            if (prices == null || prices.Count == 0 || !CheckDates(prices.Select(pr=>pr.Date).ToList()))
                throw new ArgumentException();
            return new IndicatorValue()
            {
                Date = prices.Last().Date,
                Value = prices.Select(x => x.ClosePrice).Average()
            };
        }

        public static IndicatorValue SimpleMovingAverage(IList<IndicatorValue> values)
        {
            if(values == null || values.Count == 0 || !CheckDates(values.Select(v=>v.Date).ToList()))
                throw new ArgumentException();
            return new IndicatorValue()
            {
                Date = values.Last().Date,
                Value = values.Select(x => x.Value).Average()
            };
        }

        public static IList<IndicatorValue> ExpotentialMovingAverage(IList<Price> prices, int terms)
        {
            if (prices == null || prices.Count < terms || terms < 1 || !CheckDates(prices.Select(price => price.Date).ToList()))
                throw new ArgumentException();
            IList<IndicatorValue> averages = new List<IndicatorValue>();
            IndicatorValue ema = SimpleMovingAverage(prices.Take(terms).ToList());
            averages.Add(ema);
            var alpha = 2.0m / (terms + 1);
            var p = 1 - alpha;
            for (var i = terms; i < prices.Count; i++)
            {
                var nextEma = new IndicatorValue()
                {
                    Date = prices[i].Date,
                    Value = prices[i].ClosePrice * alpha + ema.Value * p
                };
                ema = nextEma;
                averages.Add(ema);
            }
            return averages;
        }

        public static IList<IndicatorValue> ExpotentialMovingAverage(IList<IndicatorValue> values, int terms)
        {
            if (values == null || values.Count < terms || terms < 1 || !CheckDates(values.Select(price => price.Date).ToList()))
                throw new ArgumentException();
            IList<IndicatorValue> averages = new List<IndicatorValue>();
            IndicatorValue ema = SimpleMovingAverage(values.Take(terms).ToList());
            averages.Add(ema);
            var alpha = 2.0m / (terms + 1);
            var p = 1 - alpha;
            for (var i = terms; i < values.Count; i++)
            {
                var nextEma = new IndicatorValue()
                {
                    Date = values[i].Date,
                    Value = values[i].Value * alpha + ema.Value * p
                };
                ema = nextEma;
                averages.Add(ema);
            }
            return averages;
        }
    }
}
