using FluentAssertions;
using StockExchange.Business.Indicators;
using Xunit;

namespace StockExchange.UnitTest.Indicators.ADX
{
    // TODO
    public class AdxTests
    {
        private readonly AdxIndicator _indicator = new AdxIndicator();

        public AdxTests()
        {
        }

        [Fact]
        public void TestAdxValues()
        {
            var data = AdxData.GetData();
            var actualValues = AdxData.GetValues();
            var values = _indicator.Calculate(data);
            //values.ShouldAllBeEquivalentTo(actualValues);
        }
    }
}