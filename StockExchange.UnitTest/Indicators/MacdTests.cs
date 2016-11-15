using System;
using System.Collections.Generic;
using System.Linq;
using StockExchange.Business.Indicators;
using StockExchange.DataAccess.Models;
using Xunit;

namespace StockExchange.UnitTest.Indicators
{
    /// <summary>
    /// Tests for macd indicator - based on example data from MacdData.
    /// Example: http://investexcel.net/how-to-calculate-macd-in-excel/
    /// </summary>
    public class MacdTests
    {
        public static IEnumerable<object[]> DataFor26DaysEma { get; }

        public static IEnumerable<object[]> DataFor12DaysEma { get; }

        public static IEnumerable<object[]> DataFor9DaysSignalLine { get; }

        static MacdTests()
        {
            DataFor26DaysEma = new List<object[]> { new object[] { MacdData.HistorcalData, MacdData.Get26DaysEma() } };
            DataFor12DaysEma = new List<object[]> { new object[] { MacdData.HistorcalData, MacdData.Get12DaysEma() } };
            DataFor9DaysSignalLine = new List<object[]> { new object[] { MacdData.HistorcalData, MacdData.Get9DaysSignal() } };
        }

        // ReSharper disable once UnusedParameter.Local
        private static void AssertDecimals(decimal expected, decimal actual, int prec)
        {
            var diff = expected - actual;
            Assert.True(diff <= (decimal)Math.Pow(10, -prec));
        }

        [Theory]
        [MemberData(nameof(DataFor26DaysEma))]
        private void ExpotentialMovingAverage26Test(decimal[] data, decimal[] expected26DaysEma)
        {
            var actual26DaysEma = MovingAverageHelper.ExpotentialMovingAverage(data, 26);
            Assert.Equal(expected26DaysEma.Length, actual26DaysEma.Count);
            for (var i = 0; i < actual26DaysEma.Count; i++)
                AssertDecimals(expected26DaysEma[i], actual26DaysEma[i], 6);
        }

        [Theory]
        [MemberData(nameof(DataFor12DaysEma))]
        private void ExpotentialMovingAverage12Test(decimal[] data, decimal[] expected12DaysEma)
        {
            var actual12DaysEma = MovingAverageHelper.ExpotentialMovingAverage(data, 12);
            Assert.Equal(expected12DaysEma.Length, actual12DaysEma.Count);
            for (var i = 0; i < actual12DaysEma.Count; i++)
                AssertDecimals(expected12DaysEma[i], actual12DaysEma[i], 6);
        }

        [Theory]
        [MemberData(nameof(DataFor9DaysSignalLine))]
        private void SignalLineTest(decimal[] data, decimal[] expected9DaysSignalLine)
        {
            var indicator = new MacdIndicator();
            var actual9DaysSignalLine =
                indicator.CalculateSignalLine(data.Select(x => new Price() { ClosePrice = x }).ToList());
            Assert.Equal(expected9DaysSignalLine.Length, actual9DaysSignalLine.Count);
            for (var i = 0; i < expected9DaysSignalLine.Length; i++)
                AssertDecimals(expected9DaysSignalLine[i], actual9DaysSignalLine[i], 6);
        }

        //[Fact]
        //private void IntersectionTest()
        //{
        //    MacdIndicator indicator = new MacdIndicator(26,12,9);
        //    var events = indicator.Simulate(MacdData.HistorcalData).ToArray();
        //    Assert.NotNull(events);
        //    Assert.Equal(2, events.Length);
        //    Assert.Equal(SignalEventAction.Buy, events[0]);
        //    Assert.Equal(SignalEventAction.Sell, events[1]);
        //}
    }
}
