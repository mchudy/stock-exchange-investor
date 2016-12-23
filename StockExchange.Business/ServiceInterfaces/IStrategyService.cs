using StockExchange.Business.Models.Strategy;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface IStrategyService
    {
        int CreateStrategy(StrategyDto strategy);
    }
}
