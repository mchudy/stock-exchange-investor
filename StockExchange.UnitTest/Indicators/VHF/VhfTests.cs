using FluentAssertions;
using StockExchange.Business.Indicators;
using StockExchange.UnitTest.TestHelpers;
using Xunit;

namespace StockExchange.UnitTest.Indicators.VHF
{
    public class VhfTests
    {
        private readonly VhfIndicator _indicator = new VhfIndicator();

        public VhfTests()
        {
            DataHelper.SetPrecisionForDecimal(VhfData.DataPrecision);
        }

        [Fact]
        public void Test_for_default_term()
        {
            _indicator.Term = 3;
            var values = _indicator.Calculate(VhfData.HistoricalData);
            values.ShouldAllBeEquivalentTo(VhfData.Results);
        }
    }
}