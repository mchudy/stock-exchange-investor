using System.Collections.Generic;

namespace StockExchange.Business.Indicators.Common
{
    public interface IIndicatorFactory
    {
        IIndicator CreateIndicator(IndicatorType indicatorType);
        IIndicator CreateIndicator(IndicatorType indicatorType, Dictionary<string, int> parameters);
    }
}