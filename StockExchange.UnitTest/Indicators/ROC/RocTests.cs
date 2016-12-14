using FluentAssertions;
using StockExchange.Business.Indicators;
using StockExchange.DataAccess.Models;
using StockExchange.UnitTest.TestHelpers;
using System.Collections.Generic;
using Xunit;

namespace StockExchange.UnitTest.Indicators.ROC
{
    public class RocTests
    {
        private readonly RocIndicator _indicator = new RocIndicator();

        public RocTests()
        {
            DataHelper.SetPrecisionForDecimal(RocData.DataPrecision);
        }

        [Fact]
        public void Test_on_single_value_with_term_equal_to_one()
        {
            _indicator.Term = 1;
            var prices = new List<Price>
            {
                new Price {ClosePrice = 2},
                new Price {ClosePrice = 10}
            };

            var values = _indicator.Calculate(prices);

            values[0].Value.Should().Be(400);
        }

        [Fact]
        public void Test_on_sample_data_with_default_term()
        {
            var values = _indicator.Calculate(RocData.HistoricalData);

            values.ShouldAllBeEquivalentTo(RocData.Term12Results);
        }
    }
}
