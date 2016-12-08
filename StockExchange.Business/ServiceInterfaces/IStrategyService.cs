using StockExchange.Business.Models;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface IStrategyService
    {
        void CreateStrategy(StrategyDto strategy);
    }
}
