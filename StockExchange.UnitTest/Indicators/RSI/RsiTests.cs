using FluentAssertions;
using StockExchange.Business.Indicators;
using StockExchange.DataAccess.Models;
using StockExchange.UnitTest.Helpers;
using System.Collections.Generic;
using Xunit;

namespace StockExchange.UnitTest.Indicators
{
    public class RsiTests
    {
        private readonly RsiIndicator _indicator = new RsiIndicator();

        public RsiTests()
        {
            AssertionOptions.AssertEquivalencyUsing(options =>
                options.Using<decimal>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, RsiData.DataPrecision)).WhenTypeIs<decimal>());
        }

        [Fact]
        public void Test_data_with_default_term()
        {
            var values = _indicator.Calculate(RsiData.HistoricalData);

            values.ShouldAllBeEquivalentTo(RsiData.Term14Results);
        }

        [Fact]
        public void Test_data_for_nondefault_term()
        {
            _indicator.Term = 20;

            var values = _indicator.Calculate(RsiData.HistoricalData);

            values.ShouldAllBeEquivalentTo(RsiData.Term20Results);
        }

        [Fact]
        public void Should_return_number_of_prices_minus_term_results()
        {
            const int pricesCount = 30;
            _indicator.Term = 20;

            var result = _indicator.Calculate(GetEmptyPrices(pricesCount));

            result.Count.Should().Be(10);
        }

        [Fact]
        public void When_average_loss_is_zero_should_return_100()
        {
            var prices = GetEmptyPrices(15);

            var result = _indicator.Calculate(prices);

            result[0].Value.Should().Be(100);
        }

        private static List<Price> GetEmptyPrices(int count)
        {
            var prices = new List<Price>();
            for (int i = 0; i < count; i++)
            {
                prices.Add(new Price { ClosePrice = 0, Date = DataHelper.StartDate.AddDays(i) });
            }
            return prices;
        }
    }
}
