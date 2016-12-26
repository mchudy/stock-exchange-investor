using FluentAssertions;
using StockExchange.Business.Indicators;
using StockExchange.DataAccess.Models;
using StockExchange.UnitTest.TestHelpers;
using System.Collections.Generic;
using Xunit;

namespace StockExchange.UnitTest.Indicators.OBV
{
    public class ObvTests
    {
        private readonly ObvIndicator _indicator = new ObvIndicator();
        private readonly List<Price> _simpleTestPrices = new List<Price>
        {
            new Price {ClosePrice = 100, Volume = 1000},
        };

        public ObvTests()
        {
            DataHelper.SetPrecisionForDecimal(ObvData.DataPrecision);
        }

        [Fact]
        public void First_value_should_be_equal_to_volume_from_the_first_day()
        {
            var values = _indicator.Calculate(_simpleTestPrices);
            values[0].Value.Should().Be(1000);
        }

        [Fact]
        public void When_close_price_from_previous_day_has_not_changed_value_should_not_change()
        {
            _simpleTestPrices.Add(new Price { ClosePrice = 100, Volume = 2000 });
            var values = _indicator.Calculate(_simpleTestPrices);
            values[1].Value.Should().Be(1000);
        }

        [Fact]
        public void When_close_price_is_greater_than_previous_day_volume_should_be_added_to_value()
        {
            _simpleTestPrices.Add(new Price { ClosePrice = 200, Volume = 2000 });
            var values = _indicator.Calculate(_simpleTestPrices);
            values[1].Value.Should().Be(3000);
        }

        [Fact]
        public void When_close_price_is_less_than_previous_day_volume_should_be_subtracted_from_value()
        {
            _simpleTestPrices.Add(new Price { ClosePrice = 50, Volume = 2000 });
            var values = _indicator.Calculate(_simpleTestPrices);
            values[1].Value.Should().Be(-1000);
        }

        [Fact]
        public void Test_on_sample_data()
        {
            var values = _indicator.Calculate(ObvData.HistoricalData);
            values.ShouldAllBeEquivalentTo(ObvData.ExpectedResults);
        }
    }
}