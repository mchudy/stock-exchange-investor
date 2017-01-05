using StockExchange.Business.Models.Strategy;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface IStrategyService
    {
        Task<int> CreateStrategy(StrategyDto strategy);

        Task<IList<StrategyDto>> GetStrategies(int userId);

        Task<StrategyDto> GetStrategy(int userId, int strategyId);

        Task DeleteStrategy(int strategyId, int userId);

        Task UpdateStrategy(StrategyDto dto);
    }
}
