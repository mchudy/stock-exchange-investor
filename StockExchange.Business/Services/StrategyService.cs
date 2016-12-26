using StockExchange.Business.Exceptions;
using StockExchange.Business.Models.Strategy;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace StockExchange.Business.Services
{
    public class StrategyService : IStrategyService
    {
        private readonly IRepository<InvestmentStrategy> _strategiesRepository;
        private readonly IIndicatorsService _indicatorsService;

        public StrategyService(IRepository<InvestmentStrategy> strategiesRepository, IIndicatorsService indicatorsService)
        {
            _strategiesRepository = strategiesRepository;
            _indicatorsService = indicatorsService;
        }

        public IList<StrategyDto> GetUserStrategies(int userId)
        {
            return _strategiesRepository.GetQueryable()
                .Include(t => t.Indicators)
                .Where(t => t.UserId == userId)
                .OrderBy(t => t.Name)
                .Select(t => new StrategyDto
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    Name = t.Name
                }).ToList();
        }

        public int CreateStrategy(StrategyDto strategy)
        {
            if(_strategiesRepository.GetQueryable().Any(s => s.UserId == strategy.UserId && s.Name == strategy.Name))
                throw new BusinessException(nameof(strategy.Name), "Strategy with this name already exists");

            var investmentStrategy = new InvestmentStrategy
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
                    IndicatorType = (int)indicator.IndicatorType.Value,
                    Strategy = investmentStrategy,
                    Properties = new List<StrategyIndicatorProperty>()
                };
                foreach (var indicatorProperty in indicator.Properties)
                {
                    if (
                        _indicatorsService.GetPropertiesForIndicator(indicator.IndicatorType.Value)
                            .All(item => item.Name != indicatorProperty.Name)) continue;
                    strategyIndicator.Properties.Add(new StrategyIndicatorProperty
                    {
                        Indicator = strategyIndicator,
                        Name = indicatorProperty.Name,
                        Value = indicatorProperty.Value
                    });
                }
                investmentStrategy.Indicators.Add(strategyIndicator);
            }
            _strategiesRepository.Insert(investmentStrategy);
            _strategiesRepository.Save();
            return investmentStrategy.Id;
        }
    }
}
