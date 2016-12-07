using StockExchange.Business.Indicators;
using StockExchange.Business.Models.Indicators;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StockExchange.Business.Services
{
    public class IndicatorsService : IIndicatorsService
    {
        private readonly IIndicatorFactory _indicatorFactory;

        public IndicatorsService(IIndicatorFactory indicatorFactory)
        {
            _indicatorFactory = indicatorFactory;
        }

        public IList<IndicatorProperty> GetPropertiesForIndicator(IndicatorType type)
        {
            var indicator = _indicatorFactory.CreateIndicator(type);
            var properties = indicator.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.Name != nameof(IIndicator.Type));

            return properties.Select(property => new IndicatorProperty
            {
                Name = property.Name,
                Value = (int)property.GetValue(indicator)
            }).ToList();
        }
    }
}
