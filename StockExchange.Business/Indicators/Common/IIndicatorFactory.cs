namespace StockExchange.Business.Indicators.Common
{
    public interface IIndicatorFactory
    {
        IIndicator CreateIndicator(IndicatorType indicatorType);
    }
}