using System.Collections.Generic;
using StockExchange.Business.Models.Strategy;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface IStrategyService
    {
        int CreateStrategy(StrategyDto strategy);

        IList<StrategyDto> GetUserStrategies(int userId);
    }
}
