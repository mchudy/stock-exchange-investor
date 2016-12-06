using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Business.Indicators
{
    public class IndicatorProperty
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class IndicatorPropertyList
    {
        private static readonly string IndicatorSuffix = "Indicator";

        public IndicatorType IndicatorType { get; set; }
        public List<IndicatorProperty> Properties { get; set; }

        public IndicatorPropertyList(IndicatorType indicatorType)
        {
            IndicatorType = indicatorType;
            Properties = new List<IndicatorProperty>();
            string indicator = indicatorType + IndicatorSuffix;
            var type = this.GetType().Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IIndicator))).
                FirstOrDefault(t => t.Name.Equals(indicator));
            if (type == null) return;
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Properties.Add(new IndicatorProperty() { Name = property.Name });
            }
        }
    }
}
