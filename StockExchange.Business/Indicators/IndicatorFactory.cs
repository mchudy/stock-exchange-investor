﻿using StockExchange.Business.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockExchange.Business.Indicators
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
            Type type;
            if (!_types.TryGetValue(indicatorType, out type))
            {
                throw new IndicatorNotFoundException($"No indicator class found for {indicatorType} type");
            }
            return Activator.CreateInstance(type) as IIndicator;
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
