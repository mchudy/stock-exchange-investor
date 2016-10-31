using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Indicators.Lines;

namespace StockExchange.Indicators.Helpers
{
    public static class MacdHelper
    {
        public static IEnumerable<decimal> CalculateExpotentialMovingAverage(IEnumerable<decimal> historicalData, int terms)
        {
            var enumerable = historicalData as decimal[] ?? historicalData.ToArray();
            IList<decimal> begin = enumerable.Take(terms).ToList();
            if(begin.Count < terms)
                throw new ArgumentException("Historical data contains less than terms elements");
            Queue<decimal> queue = new Queue<decimal>(begin);
            yield return MovingAveragesHelper.ExpotentialMovingAverage(queue);
            foreach (var source in enumerable.Skip(terms))
            {
                queue.Enqueue(source);
                queue.Dequeue();
                yield return MovingAveragesHelper.ExpotentialMovingAverage(queue);
            }
        }

        public static PointLine CalculateMacdLine(IEnumerable<decimal> historicalData, int longTerms, int shortTerms)
        {
            int diff = longTerms - shortTerms;
            var enumerable = historicalData as decimal[] ?? historicalData.ToArray();
            var longEma = CalculateExpotentialMovingAverage(enumerable, longTerms);
            var shortEma = CalculateExpotentialMovingAverage(enumerable, shortTerms);
            return new PointLine(longEma) - new PointLine(shortEma.Skip(diff));
        }

        public static PointLine CalculateSignalLine(PointLine macdLine, int signalTerm)
        {
            return new PointLine(CalculateExpotentialMovingAverage(macdLine.Values, signalTerm));
        }
    }
}
