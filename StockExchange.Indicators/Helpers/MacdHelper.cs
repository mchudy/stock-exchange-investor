using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Indicators.Lines;

namespace StockExchange.Indicators.Helpers
{
    /// <summary>
    /// Functions that helps to calculate macd indicator.
    /// </summary>
    public static class MacdHelper
    {
        /// <summary>
        /// Calculates terms-day expotential moving average (EMA) from the given data.
        /// </summary>
        /// <param name="historicalData">Historical data.</param>
        /// <param name="terms">Number of days - the term of the average</param>
        /// <returns>The values of EMA for the given data.</returns>
        public static IEnumerable<decimal> CalculateExpotentialMovingAverage(IEnumerable<decimal> historicalData, int terms)
        {
            var enumerable = historicalData as decimal[] ?? historicalData.ToArray();
            IList<decimal> begin = enumerable.Take(terms).ToList();
            if(begin.Count < terms)
                throw new ArgumentException("Historical data contains less than terms elements");
            decimal alpha = 2.0m/(terms + 1);
            decimal p = 1 - alpha;
            var ema = MovingAveragesHelper.SimpleMovingAverage(begin);
            yield return ema;
            foreach (var source in enumerable.Skip(terms))
            {
                ema = alpha*source + ema*p;
                yield return ema;
            }
        }

        public static PointLine CalculateMacdLine(IEnumerable<decimal> historicalData, int longTerms, int shortTerms)
        {
            int diff = longTerms - shortTerms;
            var enumerable = historicalData as decimal[] ?? historicalData.ToArray();
            var longEma = CalculateExpotentialMovingAverage(enumerable, longTerms);
            var shortEma = CalculateExpotentialMovingAverage(enumerable, shortTerms);
            return new PointLine(shortEma.Skip(diff))-new PointLine(longEma);
        }

        public static PointLine CalculateSignalLine(PointLine macdLine, int signalTerm)
        {
            return new PointLine(CalculateExpotentialMovingAverage(macdLine.Values, signalTerm));
        }
    }
}
