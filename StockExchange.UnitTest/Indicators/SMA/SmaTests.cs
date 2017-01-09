using FluentAssertions;
using StockExchange.Business.Indicators;
using StockExchange.UnitTest.TestHelpers;
using Xunit;

namespace StockExchange.UnitTest.Indicators.SMA
{
    public class SmaTests
    {
        private readonly SmaIndicator _indicator = new SmaIndicator();

        public SmaTests()
        {
            DataHelper.SetPrecisionForDecimal(SmaData.DataPrecision);
        }

        [Fact]
        public void Test_for_default_term()
        {
            var values = _indicator.Calculate(SmaData.HistoricalData);
            values.ShouldAllBeEquivalentTo(SmaData.Results);
        }
    }
}