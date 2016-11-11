﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using StockExchange.Business.Indicators;
using Xunit;
using Xunit.Extensions;

namespace StockExchange.UnitTest.Indicators
{
    /// <summary>
    /// Tests for macd indicator - based on example data from MacdData.
    /// Example: http://investexcel.net/how-to-calculate-macd-in-excel/
    /// </summary>
    public class MacdTests
    {
        public static IEnumerable<object[]> DataFor26DaysEma { get; }

        static MacdTests()
        {
            DataFor26DaysEma = new List<object[]> { new object[] { MacdData.HistorcalData, MacdData.Get26DaysEma() } };
        }

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
            for(int i=0; i<actual26DaysEma.Count; i++)
                AssertDecimals(expected26DaysEma[i], actual26DaysEma[i], 6);
        }

        //[Fact]
        //private void ExpotentialMovingAverage12Test()
        //{
        //    var data = MacdData.HistorcalData;
        //    decimal[] expected12 = MacdData.Get12DaysEma().ToArray();
        //    decimal[] actual = MacdHelper.CalculateExpotentialMovingAverage(data, 12).ToArray();
        //    Assert.Equal(expected12.Length, actual.Length);
        //    for (int i = 0; i < actual.Length; i++)
        //        AssertDecimals(expected12[i], actual[i],6);
        //}

        //[Fact]
        //private void MacdLineTest()
        //{
        //    var l1 = MacdData.Get12DaysEma().Skip(26 - 12).ToArray();
        //    var l2 = MacdData.Get26DaysEma().ToArray();
        //    IList<decimal> expectedLine = new List<decimal>();
        //    for(int i=0; i<l2.Length; i++)
        //        expectedLine.Add(l1[i]-l2[i]);
        //    var actualLine = MacdHelper.CalculateMacdLine(MacdData.HistorcalData, 26, 12);
        //    Assert.NotNull(actualLine);
        //    Assert.NotNull(actualLine.Values);
        //    Assert.Equal(expectedLine.Count, actualLine.Values.Count);
        //    for(int i=0; i<expectedLine.Count; i++)
        //        AssertDecimals(expectedLine[i], actualLine.Values[i],6);
        //}

        //[Fact]
        //private void SignalLineTest()
        //{
        //    var expectedLine = MacdData.Get9DaysSignal().ToArray();
        //    var actualLine = MacdHelper.CalculateSignalLine(MacdHelper.CalculateMacdLine(MacdData.HistorcalData, 26, 12), 9);
        //    Assert.NotNull(actualLine);
        //    Assert.NotNull(actualLine.Values);
        //    Assert.Equal(expectedLine.Length, actualLine.Values.Count);
        //    for (int i = 0; i < expectedLine.Length; i++)
        //        AssertDecimals(expectedLine[i], actualLine.Values[i],6);
        //}

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