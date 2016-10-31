using System;
using System.Collections.Generic;
using System.Linq;

namespace StockExchange.Indicators.Helpers
{
    public static class MovingAveragesHelper
    {
        /// <summary>
        /// Calculates simple moving average for historical data given in parameter.
        /// It is simple arithmetical average.
        /// </summary>
        /// <param name="values">Data from last days. First element represents a value that was encountered 
        /// n-1 days ago and a last element is current value.</param>
        /// <returns>Calculated simple moving average (SMA) for the given values.</returns>
        public static decimal SimpleMovingAverage(IEnumerable<decimal> values)
        {
            if(values == null)
                throw new ArgumentNullException(nameof(values));
            return values.Average();
        }

        /// <summary>
        /// Calculates weighted moving average for historical data given in parameter.
        /// </summary>
        /// <param name="values">Data from last days. First element represents a value that was encountered 
        /// n-1 days ago and a last element is current value.</param>
        /// <returns>Calculated weighted moving average (WMA) for the given values.</returns>
        public static decimal WeightedMovingAverage(IEnumerable<decimal> values)
        {
            var valuesArray = values as decimal[] ?? values.ToArray();
            int count = valuesArray.Length;
            decimal avg = 0;
            int divider = 0;
            for (int i = 0; i < count; i++)
            {
                avg += (count - i)*valuesArray[i];
                divider += i;
            }
            return avg/divider;
        }

        /// <summary>
        /// Calculates expotential moving average for historical data given in parameter.
        /// The alpha parameter here has default value alpha = 2/(N+1)
        /// </summary>
        /// <param name="values">Data from last days. First element represents a value that was encountered 
        /// n-1 days ago and a last element is current value.</param>
        /// <returns>Calculated expotential moving average (EMA) for the given values.</returns>
        public static decimal ExpotentialMovingAverage(IEnumerable<decimal> values)
        {
            var valuesArray = values as decimal[] ?? values.ToArray();
            int count = valuesArray.Length;
            decimal p = 1 - 2.0m / (count + 1);
            decimal weight = 1;
            decimal weightSum = 0;
            decimal avg = 0;
            for (int i = 0; i < count; i++)
            {
                avg += valuesArray[i] * weight;
                weightSum += weight;
                weight *= p;
            }
            return avg / weightSum;
        }
    }
}
