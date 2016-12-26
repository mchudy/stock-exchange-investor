using FluentAssertions;
using StockExchange.Business.Indicators;
using StockExchange.Business.Indicators.Common;
using StockExchange.UnitTest.TestHelpers;
using Xunit;

namespace StockExchange.UnitTest.Indicators.PP
{
    public class PpTests
    {
        private readonly IIndicator _indicator = new PpIndicator();

        public PpTests()
        {
            DataHelper.SetPrecisionForDecimal(PpData.DataPrecision);
        }

        [Fact]
        public void Pivot_Point_On_Sample_Data_Test()
        {
            var values = _indicator.Calculate(PpData.HistoricalPrices);
            var expectedValues = PpData.PivotPointValues;
            values.ShouldAllBeEquivalentTo(expectedValues);
        } 
    }
}
