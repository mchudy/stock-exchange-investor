using FluentAssertions;
using StockExchange.Business.Indicators;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using Xunit;

namespace StockExchange.UnitTest.Indicators.ATR
{
    public class AtrTests
    {
        private readonly AtrIndicator _indicator = new AtrIndicator();

        public AtrTests()
        {
            AssertionOptions.AssertEquivalencyUsing(options =>
                options.Using<decimal>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, AtrData.DataPrecision)).WhenTypeIs<decimal>());
        }

        [Fact]
        public void Should_take_mean_average_for_the_first_day()
        {
            // RS values for the prices are 2 and 6 (high - low), so the result should be 4
            // (for the first day we take normal average)
            var prices = new List<Price>
            {
                new Price {HighPrice = 4, LowPrice = 2, ClosePrice = 5},
                new Price {HighPrice = 8, LowPrice = 2, ClosePrice = 4}
            };
            _indicator.Term = 2;

            var values = _indicator.Calculate(prices);

            values.Count.Should().Be(1);
            values[0].Value.Should().Be(4);
        }

        [Fact]
        public void Test_for_default_term()
        {
            var values = _indicator.Calculate(AtrData.HistoricalData);

            values.ShouldAllBeEquivalentTo(AtrData.Term14Results);
        }

    }
}