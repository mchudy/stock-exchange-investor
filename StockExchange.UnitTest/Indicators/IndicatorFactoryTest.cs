using FluentAssertions;
using StockExchange.Business.Exceptions;
using StockExchange.Business.Indicators;
using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Indicators;
using System;
using System.Collections.Generic;
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
        [InlineData(IndicatorType.PivotPoint, typeof(PpIndicator))]
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
            const IndicatorType type = (IndicatorType)0;
            Action act = () => _factory.CreateIndicator(type);
            act.ShouldThrow<IndicatorNotFoundException>();
        }

        [Fact]
        public void Should_assign_custom_properties_to_indicator()
        {
            var strategyIndicator = new ParameterizedIndicator()
            {
                IndicatorType = IndicatorType.Rsi,
                Properties = new List<IndicatorProperty>
                {
                    new IndicatorProperty {Name = nameof(RsiIndicator.Term), Value = 50},
                    new IndicatorProperty {Name = nameof(RsiIndicator.Maximum), Value = 99},
                    new IndicatorProperty {Name = nameof(RsiIndicator.Minimum), Value = 10}
                }
            };

            var indicator = (RsiIndicator)_factory.CreateIndicator(strategyIndicator);

            indicator.Minimum.Should().Be(10);
            indicator.Maximum.Should().Be(99);
            indicator.Term.Should().Be(50);
        }

        [Fact]
        public void Should_throw_exception_given_incorrect_property()
        {
            var strategyIndicator = new ParameterizedIndicator
            {
                IndicatorType = IndicatorType.Ema,
                Properties = new List<IndicatorProperty>
                {
                    new IndicatorProperty {Name = "aaa", Value = 50},
                }
            };

            Action act = () => _factory.CreateIndicator(strategyIndicator);

            act.ShouldThrow<ArgumentException>();
        }
    }
}
