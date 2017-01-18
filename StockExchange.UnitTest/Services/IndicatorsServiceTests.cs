using FluentAssertions;
using Moq;
using StockExchange.Business.Indicators;
using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Indicators;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Business.Services;
using StockExchange.DataAccess.Cache;
using System;
using System.Collections.Generic;
using Xunit;

namespace StockExchange.UnitTest.Services
{
    public class IndicatorsServiceTests
    {
        private readonly IIndicatorsService _service;

        private readonly Mock<IIndicatorFactory> _factory = new Mock<IIndicatorFactory>();
        private readonly Mock<IPriceService> _pricesService = new Mock<IPriceService>();
        private readonly Mock<ICompanyService> _companyService = new Mock<ICompanyService>();
        private readonly Mock<ICache> _cache = new Mock<ICache>();

        public IndicatorsServiceTests()
        {
            _service = new IndicatorsService(_factory.Object, _pricesService.Object, 
                _companyService.Object, _cache.Object);
        }

        [Fact]
        public void Are_Macd_indicator_properties_correct()
        {
            const IndicatorType type = IndicatorType.Macd;
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
            const IndicatorType type = IndicatorType.Macd;
            _factory.Setup(f => f.CreateIndicator(type)).Returns(new RsiIndicator());
            var properties = _service.GetPropertiesForIndicator(type);
            properties.Count.Should().Be(3);
            properties.Should().Contain(p => p.Name == "Term" && p.Value == RsiIndicator.DefaultRsiTerm);
        }

        [Fact]
        public void Getting_available_indicators_returns_correct_values()
        {
            var availableIndicators = _service.GetAllIndicatorTypes();
            availableIndicators.Should().NotBeNull();
            availableIndicators.Should().Contain(IndicatorType.Macd);
            availableIndicators.Should().Contain(IndicatorType.Rsi);
            availableIndicators.Should().Contain(IndicatorType.PivotPoint);
            availableIndicators.Should().Contain(IndicatorType.Atr);
            availableIndicators.Should().Contain(IndicatorType.Roc);
            availableIndicators.Should().Contain(IndicatorType.Obv);
            availableIndicators.Should().Contain(IndicatorType.Sma);
            availableIndicators.Should().Contain(IndicatorType.Ema);
            availableIndicators.Should().Contain(IndicatorType.Vhf);
            availableIndicators.Should().Contain(IndicatorType.Vroc);
            availableIndicators.Should().Contain(IndicatorType.Vpt);
            availableIndicators.Should().Contain(IndicatorType.Adx);
        }

        [Fact]
        public void GetSignals_should_create_indicator_with_custom_properties()
        {
            var indicator = new ParameterizedIndicator
            {
                IndicatorType = IndicatorType.Ema,
                Properties = new List<IndicatorProperty>
                {
                    new IndicatorProperty {Name = nameof(EmaIndicator.Term), Value = 10}
                }
            };
            _factory.Setup(f => f.CreateIndicator(IndicatorType.Ema, new Dictionary<string, int> { { nameof(EmaIndicator.Term), 10 } }))
                .Returns(new EmaIndicator { Term = 10 });

            _service.GetSignals(new DateTime(2016, 1, 1), new DateTime(2016, 1, 30), new List<int> { 1 },
                new List<ParameterizedIndicator> { indicator });

            _factory.Verify(f => f.CreateIndicator(IndicatorType.Ema, new Dictionary<string, int> { { nameof(EmaIndicator.Term), 10 } }));
        }
    }
}
