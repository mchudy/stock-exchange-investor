namespace StockExchange.Business.Indicators
{
    public interface IIndicatorFactory
    {
        IIndicator CreateIndicator(IndicatorType indicatorType);
    }
}