using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using StockExchange.Indicators;
using StockExchange.Indicators.Helpers;
using Xunit;

namespace StockExchange.UnitTest.Indicators
{
    /// <summary>
    /// Tests for macd indicator - based on example data from MacdData.
    /// Example: http://investexcel.net/how-to-calculate-macd-in-excel/
    /// </summary>
    public class MacdTests
    {
        [Fact]
        private void ExpotentialMovingAverage26Test()
        {
            var data = MacdData.HistorcalData;
            decimal[] expected26 = MacdData.Get26DaysEma().ToArray();
            decimal[] actual = MacdHelper.CalculateExpotentialMovingAverage(data, 26).ToArray();
            Assert.Equal(expected26.Length, actual.Length);
            for(int i=0; i<actual.Length; i++)
                AssertDecimals(expected26[i], actual[i], 6);
        }

        [Fact]
        private void ExpotentialMovingAverage12Test()
        {
            var data = MacdData.HistorcalData;
            decimal[] expected12 = MacdData.Get12DaysEma().ToArray();
            decimal[] actual = MacdHelper.CalculateExpotentialMovingAverage(data, 12).ToArray();
            Assert.Equal(expected12.Length, actual.Length);
            for (int i = 0; i < actual.Length; i++)
                AssertDecimals(expected12[i], actual[i],6);
        }

        [Fact]
        private void MacdLineTest()
        {
            var l1 = MacdData.Get12DaysEma().Skip(26 - 12).ToArray();
            var l2 = MacdData.Get26DaysEma().ToArray();
            IList<decimal> expectedLine = new List<decimal>();
            for(int i=0; i<l2.Length; i++)
                expectedLine.Add(l1[i]-l2[i]);
            var actualLine = MacdHelper.CalculateMacdLine(MacdData.HistorcalData, 26, 12);
            Assert.NotNull(actualLine);
            Assert.NotNull(actualLine.Values);
            Assert.Equal(expectedLine.Count, actualLine.Values.Count);
            for(int i=0; i<expectedLine.Count; i++)
                AssertDecimals(expectedLine[i], actualLine.Values[i],6);
        }

        [Fact]
        private void SignalLineTest()
        {
            var expectedLine = MacdData.Get9DaysSignal().ToArray();
            var actualLine = MacdHelper.CalculateSignalLine(MacdHelper.CalculateMacdLine(MacdData.HistorcalData, 26, 12), 9);
            Assert.NotNull(actualLine);
            Assert.NotNull(actualLine.Values);
            Assert.Equal(expectedLine.Length, actualLine.Values.Count);
            for (int i = 0; i < expectedLine.Length; i++)
                AssertDecimals(expectedLine[i], actualLine.Values[i],6);
        }

        private void AssertDecimals(decimal expected, decimal actual, int prec)
        {
            var diff = expected - actual;
            Assert.True(diff<=(decimal)Math.Pow(10, -prec));
        }
    }
}
