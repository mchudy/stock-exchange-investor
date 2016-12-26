using FluentAssertions;
using StockExchange.Business.Indicators;
using StockExchange.UnitTest.TestHelpers;
using Xunit;

namespace StockExchange.UnitTest.Indicators.EMA
{
    public class EmaTests
    {
        private readonly EmaIndicator _indicator = new EmaIndicator();

        public EmaTests()
        {
            DataHelper.SetPrecisionForDecimal(EmaData.DataPrecision);
        }

        [Fact]
        public void Test_for_default_term()
        {
            var values = _indicator.Calculate(EmaData.HistoricalData);
            values.ShouldAllBeEquivalentTo(EmaData.Results);
        }
    }
}