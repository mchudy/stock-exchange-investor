using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Business.Indicators.Common;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Business.Services;
using Xunit;

namespace StockExchange.UnitTest.Indicators
{
    public class IndicatorDescriptionTests
    {
        private readonly IIndicatorsService _indicatorsService;

        public IndicatorDescriptionTests()
        {
            _indicatorsService = new IndicatorsService(new IndicatorFactory(), null, null, null);
        }

        [Fact]
        public void Should_Indicator_Has_Non_Empty_Descriptions()
        {
            foreach (var indicatorType in _indicatorsService.GetAllIndicatorTypes())
            {
                var description = _indicatorsService.GetIndicatorDescription(indicatorType);
                Assert.NotNull(description);
                Assert.False(string.IsNullOrEmpty(description.IndicatorDescription));
                Assert.False(string.IsNullOrEmpty(description.BuySignalDescription));
                Assert.False(string.IsNullOrEmpty(description.SellSignalDescription));
            }
        }
    }
}
