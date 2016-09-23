namespace StockExchange.Common
{
    public interface IFactory<out T>
    {
        T CreateInstance();
    }
}
