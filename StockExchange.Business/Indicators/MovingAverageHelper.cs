using System;
using System.Collections.Generic;
using System.Linq;

namespace StockExchange.Business.Indicators
{
    public static class MovingAverageHelper
    {
        /// <summary>
        /// Calculate Simple Moving Average for the given values.
        /// </summary>
        /// <param name="values">Data for calculations.</param>
        /// <returns>Simple Moving Average (SMA) which is the arithmetical average of given values.</returns>
        public static decimal SimpleMovingAverage(IList<decimal> values)
        {
            if (values == null || values.Count == 0)
                throw new ArgumentException();
            return values.Average();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="terms"></param>
        /// <returns></returns>
        public static IList<decimal> SimpleMovingAverage(IList<decimal> values, int terms)
        {
            if (values == null || values.Count < terms || terms < 1)
                throw new ArgumentException();
            IList<decimal> averages = new List<decimal>();
            var avg = values.Take(terms).Average();
            averages.Add(avg);
            for (var i = terms; i < values.Count; i++)
            {
                avg = avg + (values[i] - values[i - terms]) / terms;
                averages.Add(avg);
            }
            return averages;
        }

        public static IList<decimal> ExpotentialMovingAverage(IList<decimal> values, int terms)
        {
            if (values == null || values.Count < terms || terms < 1)
                throw new ArgumentException();
            IList<decimal> averages = new List<decimal>();
            var ema = values.Take(terms).Average();
            averages.Add(ema);
            var alpha = 2.0m / (terms + 1);
            var p = 1 - alpha;
            for (var i = terms; i < values.Count; i++)
            {
                ema = values[i] * alpha + ema * p;
                averages.Add(ema);
            }
            return averages;
        }
    }
}
