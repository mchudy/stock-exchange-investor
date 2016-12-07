using FluentAssertions;
using Moq;
using StockExchange.Business.Indicators;
using StockExchange.Business.Services;
using System.Linq;
using Xunit;

namespace StockExchange.UnitTest.Services
{
    public class IndicatorsServiceTests
    {
        private readonly Mock<IIndicatorFactory> _factory = new Mock<IIndicatorFactory>();
        private readonly IIndicatorsService _service;

        public IndicatorsServiceTests()
        {
            _service = new IndicatorsService(_factory.Object);
        }

        [Fact]
        public void Are_Macd_indicator_properties_correct()
        {
            var type = IndicatorType.Macd;
            _factory.Setup(f => f.CreateIndicator(type)).Returns(new MacdIndicator());

            var properties = _service.GetPropertiesForIndicator(type);

            properties.Count.Should().Be(3);
            properties.Any(p => p.Name == "LongTerm" && p.Value == MacdIndicator.DefaultLongTerm);
            properties.Any(p => p.Name == "ShortTerm" && p.Value == MacdIndicator.DefaultShortTerm);
            properties.Any(p => p.Name == "SignalTerm" && p.Value == MacdIndicator.DefaultSignalTerm);
        }

        [Fact]
        public void Are_Rsi_indicator_properties_correct()
        {
            var type = IndicatorType.Macd;
            _factory.Setup(f => f.CreateIndicator(type)).Returns(new RsiIndicator());

            var properties = _service.GetPropertiesForIndicator(type);

            properties.Count.Should().Be(1);
            properties.Any(p => p.Name == "Term" && p.Value == RsiIndicator.DefaultRsiTerm);
        }
    }
}
