using FluentAssertions;
using StockExchange.Business.Indicators;
using StockExchange.Business.Models;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
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
            DataFor26DaysEma = new List<object[]> { new object[] { MacdData.HistoricalData, MacdData.Ema26DaysResults } };
            DataFor12DaysEma = new List<object[]> { new object[] { MacdData.HistoricalData, MacdData.Ema12DaysResults } };
            DataFor9DaysSignalLine = new List<object[]> { new object[] { MacdData.HistoricalData, MacdData.Signal9DaysResults } };

            AssertionOptions.AssertEquivalencyUsing(options =>
                options.Using<decimal>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, MacdData.DataPrecision)).WhenTypeIs<decimal>());
        }

        [Theory]
        [MemberData(nameof(DataFor26DaysEma))]
        public void ExpotentialMovingAverage26Test(IList<Price> data, IList<IndicatorValue> expected26DaysEma)
        {
            var actual26DaysEma = MovingAverageHelper.ExpotentialMovingAverage(data, 26);

            actual26DaysEma.ShouldAllBeEquivalentTo(expected26DaysEma);
        }

        [Theory]
        [MemberData(nameof(DataFor12DaysEma))]
        public void ExpotentialMovingAverage12Test(IList<Price> data, IList<IndicatorValue> expected12DaysEma)
        {
            var actual12DaysEma = MovingAverageHelper.ExpotentialMovingAverage(data, 12);

            actual12DaysEma.ShouldAllBeEquivalentTo(expected12DaysEma);
        }

        [Theory]
        [MemberData(nameof(DataFor9DaysSignalLine))]
        public void SignalLineTest(IList<Price> data, IList<IndicatorValue> expected9DaysSignalLine)
        {
            var indicator = new MacdIndicator();

            var actual9DaysSignalLine = indicator.Calculate(data);

            Assert.Equal(expected9DaysSignalLine.Count, actual9DaysSignalLine.Count);
            for (var i = 0; i < expected9DaysSignalLine.Count; i++)
            {
                var doubleLineIndicator = (DoubleLineIndicatorValue)actual9DaysSignalLine[i];
                Assert.Equal(expected9DaysSignalLine[i].Date, doubleLineIndicator.Date);
                Assert.Equal(expected9DaysSignalLine[i].Value, doubleLineIndicator.SecondLineValue, MacdData.DataPrecision);
            }
        }
    }
}
