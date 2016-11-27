using FluentAssertions;
using StockExchange.Business.Indicators;
using StockExchange.Business.Models;
using Xunit;

namespace StockExchange.UnitTest.Indicators
{
    /// <summary>
    /// Tests for macd indicator - based on example data from MacdData.
    /// Example: http://investexcel.net/how-to-calculate-macd-in-excel/
    /// </summary>
    public class MacdTests
    {
        public MacdTests()
        {
            AssertionOptions.AssertEquivalencyUsing(options =>
                options.Using<decimal>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, MacdData.DataPrecision)).WhenTypeIs<decimal>());
        }

        [Fact]
        public void ExpotentialMovingAverage26Test()
        {
            var actual26DaysEma = MovingAverageHelper.ExpotentialMovingAverage(MacdData.HistoricalData, 26);

            actual26DaysEma.ShouldAllBeEquivalentTo(MacdData.Ema26DaysResults);
        }

        [Fact]
        public void ExpotentialMovingAverage12Test()
        {
            var actual12DaysEma = MovingAverageHelper.ExpotentialMovingAverage(MacdData.HistoricalData, 12);

            actual12DaysEma.ShouldAllBeEquivalentTo(MacdData.Ema12DaysResults);
        }

        [Fact]
        public void SignalLineTest()
        {
            var indicator = new MacdIndicator();

            var actual9DaysSignalLine = indicator.Calculate(MacdData.HistoricalData);

            var expected9DaysSignalLine = MacdData.Signal9DaysResults;
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
