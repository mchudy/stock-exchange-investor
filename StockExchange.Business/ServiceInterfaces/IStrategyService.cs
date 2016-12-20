using StockExchange.Business.Models;
using StockExchange.Business.Models.Strategy;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface IStrategyService
    {
        void CreateStrategy(CreateStrategyDto strategy);
    }
}
