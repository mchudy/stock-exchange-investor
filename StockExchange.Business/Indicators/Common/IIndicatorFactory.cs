using StockExchange.Business.Models.Indicators;

namespace StockExchange.Business.Indicators.Common
{
    public interface IIndicatorFactory
    {
        IIndicator CreateIndicator(IndicatorType indicatorType);
        IIndicator CreateIndicator(ParameterizedIndicator indicatorType);
    }
}