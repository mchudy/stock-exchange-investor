using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Business.Indicators.Common
{
    /// <summary>
    /// Marks the property that is not an indicator parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IngoreIndicatorPropertyAttribute : Attribute
    {
    }
}
