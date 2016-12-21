using System.Collections.Generic;
using System.Linq;
using StockExchange.Business.Models;
using StockExchange.Business.Models.Strategy;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;

namespace StockExchange.Business.Services
{
    public class StrategyService : IStrategyService
    {
        private readonly IRepository<InvestmentStrategy> _strategiesRepository;

        public StrategyService(IRepository<InvestmentStrategy> strategiesRepository)
        {
            _strategiesRepository = strategiesRepository;
        }

        //TODO: More information about the operation status.
        public bool CreateStrategy(CreateStrategyDto strategy)
        {
            if(_strategiesRepository.GetQueryable().Where(s=>s.UserId == strategy.UserId).Any(s=>s.Name == strategy.Name))
                return false; //Strategy name unique per user
            InvestmentStrategy investmentStrategy = new InvestmentStrategy()
            {
                UserId = strategy.UserId,
                Name = strategy.Name,
                Indicators = new List<StrategyIndicator>()
            };
            foreach (var indicator in strategy.Indicators)
            {
                int id = indicator.IndicatorType.HasValue ? (int)indicator.IndicatorType.Value : -1;
                if(id<0)
                    return false;
                var strategyIndicator = new StrategyIndicator()
                {
                    IndicatorType = id,
                    Strategy = investmentStrategy,
                    Properties = new List<StrategyIndicatorProperty>()
                };
                foreach (var indicatorProperty in indicator.Properties)
                {
                    strategyIndicator.Properties.Add(new StrategyIndicatorProperty()
                    {
                        Indicator = strategyIndicator, 
                        Name = indicatorProperty.Name,
                        Value = indicatorProperty.Value
                    });
                }
                investmentStrategy.Indicators.Add(strategyIndicator);
            }
            //_strategiesRepository.Insert(investmentStrategy);
            //_strategiesRepository.Save();
            return true;
        }
    }
}
