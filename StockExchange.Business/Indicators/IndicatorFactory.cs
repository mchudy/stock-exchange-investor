using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Business.Indicators
{
    public class IndicatorFactory
    {
        private static readonly string IndicatorSuffix = "Indicator";

        public IIndicator CreateIndicator(IndicatorType indicatorType)
        {
            var typeName =
                this.GetType()
                    .Assembly.GetTypes()
                    .Where(t => t.GetInterfaces().Contains(typeof(IIndicator)))
                    .FirstOrDefault(t => t.Name.Equals(indicatorType + IndicatorSuffix));
            if (typeName == null) return null;
            IIndicator indicator = null;
            try
            {
                indicator = Activator.CreateInstance(typeName) as IIndicator;
            }
            catch
            {
                // ignored
            }
            return indicator;
        }
    }
}
