using FluentAssertions;
using StockExchange.Business.Indicators;
using StockExchange.UnitTest.TestHelpers;
using Xunit;

namespace StockExchange.UnitTest.Indicators.ADX
{
    // TODO
    public class AdxTests
    {
        private readonly AdxIndicator _indicator = new AdxIndicator();

        public AdxTests()
        {
            DataHelper.SetPrecisionForDecimal(AdxData.AdxPrecision);
        }

        [Fact]
        public void TestAdxValues()
        {
            var data = AdxData.GetData();
            var actualValues = AdxData.GetValues();
            var values = _indicator.Calculate(data);
            Assert.Equal(actualValues.Count, values.Count);
            for(int i=0; i<values.Count; i++)
            {
                Assert.Equal(actualValues[i].Value, values[i].Value, AdxData.AdxPrecision);
            }
        }
    }
}