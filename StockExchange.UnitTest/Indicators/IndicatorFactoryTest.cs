using FluentAssertions;
using StockExchange.Business.Exceptions;
using StockExchange.Business.Indicators;
using System;
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
        [InlineData(IndicatorType.Sma, typeof(SmaIndicator))]
        [InlineData(IndicatorType.Ema, typeof(EmaIndicator))]
        [InlineData(IndicatorType.Vroc, typeof(VrocIndicator))]
        [InlineData(IndicatorType.Vhf, typeof(VhfIndicator))]
        [InlineData(IndicatorType.PivotPoint, typeof(PivotPointIndicator))]
        [InlineData(IndicatorType.Vpt, typeof(VptIndicator))]
        [InlineData(IndicatorType.Adx, typeof(AdxIndicator))]
        public void Should_create_correct_IIndicator_instance(IndicatorType type, Type resultType)
        {
            var indicator = _factory.CreateIndicator(type);
            indicator.Should().NotBeNull();
            indicator.GetType().Should().Be(resultType);
        }

        [Fact]
        public void Given_nonexistent_type_should_throw_exception()
        {
            var type = (IndicatorType)0;
            Action act = () => _factory.CreateIndicator(type);

            act.ShouldThrow<IndicatorNotFoundException>();
        }
    }
}
