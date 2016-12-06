using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using StockExchange.Business.Indicators;
using Xunit;

namespace StockExchange.UnitTest.Indicators
{
    public class IndicatorFactoryTests
    {
        private readonly IndicatorFactory _factory = new IndicatorFactory();

        [Theory]
        [InlineData(IndicatorType.Macd, typeof(MacdIndicator))]
        [InlineData(IndicatorType.Rsi, typeof(RsiIndicator))]
        [InlineData(IndicatorType.Roc, typeof(RocIndicator))]
        [InlineData(IndicatorType.Obv, typeof(ObvIndicator))]
        [InlineData(IndicatorType.Atr, typeof(AtrIndicator))]
        public void Is_Indicator_Factory_Create_Correct_Instance(IndicatorType type, Type resultType)
        {
            var indicator = _factory.CreateIndicator(type);
            indicator.Should().NotBeNull();
            indicator.GetType().Should().Be(resultType);
        }
    }
}
