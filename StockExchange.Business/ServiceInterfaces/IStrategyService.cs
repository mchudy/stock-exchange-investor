using StockExchange.Business.Models.Strategy;
using System.Collections.Generic;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface IStrategyService
    {
        int CreateStrategy(StrategyDto strategy);

        IList<StrategyDto> GetUserStrategies(int userId);

        StrategyDto GetUserStrategy(int userId, int strategyId);

        void DeleteStrategy(int strategyId, int userId);
    }
}
