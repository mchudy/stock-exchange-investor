using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using StockExchange.Business.Indicators;
using Xunit;

namespace StockExchange.UnitTest.Indicators
{
    public class IndicatorTypeReflectionTest
    {
        [Fact]
        public void Are_Macd_Indicator_Properties_Correct()
        {
            IndicatorType type = IndicatorType.Macd;
            IndicatorPropertyList properities = new IndicatorPropertyList(type);
            properities.Properties.Should().NotBeNull();
            properities.Properties.Count.Should().Be(3);
            properities.IndicatorType.Should().Be(type);
            properities.Properties.Any(p => p.Name.Equals("LongTerm"));
            properities.Properties.Any(p => p.Name.Equals("ShortTerm"));
            properities.Properties.Any(p => p.Name.Equals("SignalTerm"));
        }

        [Fact]
        public void Are_Rsi_Indicator_Properties_Correct()
        {
            IndicatorType type = IndicatorType.Rsi;
            IndicatorPropertyList properities = new IndicatorPropertyList(type);
            properities.Properties.Should().NotBeNull();
            properities.Properties.Count.Should().Be(1);
            properities.IndicatorType.Should().Be(type);
            properities.Properties.Any(p => p.Name.Equals("Term"));
        }
    }
}
