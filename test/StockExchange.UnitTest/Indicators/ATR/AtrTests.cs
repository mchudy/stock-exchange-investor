using FluentAssertions;
using StockExchange.Business.Indicators;
using StockExchange.DataAccess.Models;
using StockExchange.UnitTest.TestHelpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace StockExchange.UnitTest.Indicators.ATR
{
    public class AtrTests
    {
        private readonly AtrIndicator _indicator = new AtrIndicator();

        public AtrTests()
        {
            DataHelper.SetPrecisionForDecimal(AtrData.DataPrecision);
        }

        [Fact]
        public void Should_take_mean_average_for_the_first_day()
        {
            // RS values for the prices are 2 and 6 (high - low), so the result should be 4
            // (for the first day we take normal average)
            var date = new DateTime(2016, 10, 1);
            var prices = new List<Price>
            {
                new Price {HighPrice = 4, LowPrice = 2, ClosePrice = 5, Date = date},
                new Price {HighPrice = 8, LowPrice = 2, ClosePrice = 4, Date = date.AddDays(1)}
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