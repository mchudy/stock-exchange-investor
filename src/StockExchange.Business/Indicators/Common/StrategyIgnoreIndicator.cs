using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Business.Indicators.Common
{
    /// <summary>
    /// Marks an indicator that is not available in strategy and simulation mode.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
    public class StrategyIgnoreIndicator : Attribute
    {
    }
}
