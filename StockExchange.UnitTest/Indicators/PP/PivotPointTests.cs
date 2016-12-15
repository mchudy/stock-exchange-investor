using FluentAssertions;
using StockExchange.Business.Indicators;
using StockExchange.UnitTest.TestHelpers;
using Xunit;

namespace StockExchange.UnitTest.Indicators.PP
{
    public class PivotPointTests
    {
        private readonly IIndicator _indicator = new PivotPointIndicator();

        public PivotPointTests()
        {
            DataHelper.SetPrecisionForDecimal(PivotPointData.DataPrecision);
        }

        [Fact]
        public void Pivot_Point_On_Sample_Data_Test()
        {
            var values = _indicator.Calculate(PivotPointData.HistoricalPrices);
            var expectedValues = PivotPointData.PivotPointValues;

            values.ShouldAllBeEquivalentTo(expectedValues);
        } 
    }
}
