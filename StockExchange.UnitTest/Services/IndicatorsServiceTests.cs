using FluentAssertions;
using Moq;
using StockExchange.Business.Indicators;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Business.Services;
using Xunit;

namespace StockExchange.UnitTest.Services
{
    public class IndicatorsServiceTests
    {
        private readonly Mock<IIndicatorFactory> _factory = new Mock<IIndicatorFactory>();
        private readonly Mock<IPriceService> _pricesService = new Mock<IPriceService>();
        private readonly IIndicatorsService _service;

        public IndicatorsServiceTests()
        {
            _service = new IndicatorsService(_factory.Object, _pricesService.Object);
        }

        [Fact]
        public void Are_Macd_indicator_properties_correct()
        {
            var type = IndicatorType.Macd;
            _factory.Setup(f => f.CreateIndicator(type)).Returns(new MacdIndicator());

            var properties = _service.GetPropertiesForIndicator(type);

            properties.Count.Should().Be(3);
            properties.Should().Contain(p => p.Name == "LongTerm" && p.Value == MacdIndicator.DefaultLongTerm);
            properties.Should().Contain(p => p.Name == "ShortTerm" && p.Value == MacdIndicator.DefaultShortTerm);
            properties.Should().Contain(p => p.Name == "SignalTerm" && p.Value == MacdIndicator.DefaultSignalTerm);
        }

        [Fact]
        public void Are_Rsi_indicator_properties_correct()
        {
            var type = IndicatorType.Macd;
            _factory.Setup(f => f.CreateIndicator(type)).Returns(new RsiIndicator());

            var properties = _service.GetPropertiesForIndicator(type);

            properties.Count.Should().Be(3);
            properties.Should().Contain(p => p.Name == "Term" && p.Value == RsiIndicator.DefaultRsiTerm);
        }

        [Fact]
        public void Getting_available_indicators_returns_correct_values()
        {
            var availableIndicators = _service.GetAvailableIndicators();

            availableIndicators.Should().NotBeNull();
            availableIndicators.Should().Contain(IndicatorType.Macd);
            availableIndicators.Should().Contain(IndicatorType.Rsi);
            availableIndicators.Should().Contain(IndicatorType.PP);
            availableIndicators.Should().Contain(IndicatorType.Atr);
            availableIndicators.Should().Contain(IndicatorType.Roc);
        }
    }
}
