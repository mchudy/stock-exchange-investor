using StockExchange.Business.Exceptions;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockExchange.Business.Indicators.Common
{
    public class IndicatorFactory : IIndicatorFactory
    {
        private readonly IDictionary<IndicatorType, Type> _types = new Dictionary<IndicatorType, Type>();

        public IndicatorFactory()
        {
            LoadTypes();
        }

        public IIndicator CreateIndicator(IndicatorType indicatorType)
        {
            var type = GetIndicatorType(indicatorType);
            return Activator.CreateInstance(type) as IIndicator;
        }

        public IIndicator CreateIndicator(StrategyIndicator strategyIndicator)
        {
            if(!Enum.IsDefined(typeof(IndicatorType), strategyIndicator.IndicatorType))
                throw new IndicatorNotFoundException($"Invalid indicator type {strategyIndicator.IndicatorType} in strategy {strategyIndicator.StrategyId}");

            var indicator = CreateIndicator((IndicatorType) strategyIndicator.IndicatorType);
            Type type = indicator.GetType();
            foreach (var property in strategyIndicator.Properties)
            {
                type.GetProperty(property.Name).SetValue(indicator, property.Value);
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
