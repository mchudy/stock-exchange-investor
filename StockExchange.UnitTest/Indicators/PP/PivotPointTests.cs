using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using StockExchange.Business.Indicators;
using StockExchange.UnitTest.Indicators.ATR;
using Xunit;

namespace StockExchange.UnitTest.Indicators.PP
{
    public class PivotPointTests
    {
        private readonly IIndicator _indicator;
        public const int DataPrecission = 7;
    
        public PivotPointTests()
        {
            _indicator = new IndicatorFactory().CreateIndicator(IndicatorType.PP);
            AssertionOptions.AssertEquivalencyUsing(options =>
                options.Using<decimal>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, DataPrecission)).WhenTypeIs<decimal>());
        }

        [Fact]
        public void Pivot_Point_On_Sample_Data_Test()
        {
            var values = _indicator.Calculate(PivotPointData.HistoricalPrices);
            var expectedValues = PivotPointData.PivotPointValues;
            values.Count.Should().Be(expectedValues.Count);
            for (int i = 0; i < expectedValues.Count; i++)
                Assert.Equal(expectedValues[i].Value,values[i].Value, DataPrecission);
        } 
    }
}
