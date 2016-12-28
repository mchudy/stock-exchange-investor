﻿using StockExchange.Business.Exceptions;
using StockExchange.Business.Models.Indicators;
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

        public IIndicator CreateIndicator(ParameterizedIndicator parameterizedIndicator)
        {
            if (!parameterizedIndicator.IndicatorType.HasValue)
                throw new ArgumentNullException(nameof(parameterizedIndicator.IndicatorType));

            var indicator = CreateIndicator(parameterizedIndicator.IndicatorType.Value);
            Type type = indicator.GetType();
            foreach (var property in parameterizedIndicator.Properties)
            {
                var prop = type.GetProperty(property.Name);
                if(prop == null)
                    throw new ArgumentException($"Nonexistent property {property.Name} for indicator {indicator.Type}");
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
