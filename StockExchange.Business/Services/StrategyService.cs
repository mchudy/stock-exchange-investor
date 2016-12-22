using StockExchange.Business.Exceptions;
using StockExchange.Business.Models.Strategy;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace StockExchange.Business.Services
{
    public class StrategyService : IStrategyService
    {
        private readonly IRepository<InvestmentStrategy> _strategiesRepository;

        public StrategyService(IRepository<InvestmentStrategy> strategiesRepository)
        {
            _strategiesRepository = strategiesRepository;
        }

        public void CreateStrategy(CreateStrategyDto strategy)
        {
            if(_strategiesRepository.GetQueryable().Where(s=>s.UserId == strategy.UserId).Any(s=>s.Name == strategy.Name))
                throw new BusinessException(nameof(strategy.Name), "Strategy with this name already exists");

            InvestmentStrategy investmentStrategy = new InvestmentStrategy
            {
                UserId = strategy.UserId,
                Name = strategy.Name,
                Indicators = new List<StrategyIndicator>()
            };
            foreach (var indicator in strategy.Indicators)
            {
                if (!indicator.IndicatorType.HasValue)
                    throw new BusinessException("Wrong indicator value");

                var strategyIndicator = new StrategyIndicator
                {
                    IndicatorType = (int) indicator.IndicatorType.Value,
                    Strategy = investmentStrategy,
                    Properties = new List<StrategyIndicatorProperty>()
                };
                foreach (var indicatorProperty in indicator.Properties)
                {
                    strategyIndicator.Properties.Add(new StrategyIndicatorProperty
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
        }
    }
}
