using StockExchange.Business.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockExchange.Business.Indicators.Common
{
    /// <summary>
    /// A factory for <see cref="IIndicator"/> instances
    /// </summary>
    public class IndicatorFactory : IIndicatorFactory
    {
        private readonly IDictionary<IndicatorType, Type> _types = new Dictionary<IndicatorType, Type>();

        /// <summary>
        /// Creates a new instance of the <see cref="IndicatorFactory"/>
        /// </summary>
        public IndicatorFactory()
        {
            LoadTypes();
        }

        /// <inheritdoc />
        public IIndicator CreateIndicator(IndicatorType indicatorType)
        {
            var type = GetIndicatorType(indicatorType);
            return Activator.CreateInstance(type) as IIndicator;
        }

        /// <inheritdoc />
        public IIndicator CreateIndicator(IndicatorType indicatorType, Dictionary<string, int> properties)
        {
            var indicator = CreateIndicator(indicatorType);
            Type type = indicator.GetType();
            foreach (var property in properties)
            {
                var prop = type.GetProperty(property.Key);
                if (prop == null)
                    throw new IndicatorArgumentException($"Nonexistent property {property.Key} for indicator {indicator.Type}");
                prop.SetValue(indicator, property.Value);
            }
            return indicator;
        }

        private Type GetIndicatorType(IndicatorType indicatorType)
        {
            Type type;
            if (!_types.TryGetValue(indicatorType, out type))
            {
                throw new IndicatorNotFoundException($"No indicator class found for {indicatorType} type");
            }
            return type;
        }

        private void LoadTypes()
        {
            var indicatorTypes = GetType().Assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface
                    && typeof(IIndicator).IsAssignableFrom(t));
            foreach (var type in indicatorTypes)
            {
                var indicator = Activator.CreateInstance(type) as IIndicator;
                if (indicator != null)
                {
                    _types.Add(indicator.Type, type);
                }
            }
        }
    }
}
