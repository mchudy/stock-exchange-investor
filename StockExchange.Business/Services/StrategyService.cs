using StockExchange.Business.ErrorHandling;
using StockExchange.Business.Exceptions;
using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Indicators;
using StockExchange.Business.Models.Strategy;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
                .Where(t => t.UserId == userId && !t.IsDeleted)
                .OrderBy(t => t.Name)
                .Select(t => new StrategyDto
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    Name = t.Name,
                    Indicators = t.Indicators.Select(i => new ParameterizedIndicator
                    {
                        IndicatorType = (IndicatorType?)i.IndicatorType
                    }).ToList()
                }).ToList();
        }

        public StrategyDto GetUserStrategy(int userId, int strategyId)
        {
            var ret =
                _strategiesRepository
                    .GetQueryable().FirstOrDefault(item => item.Id == strategyId && item.UserId == userId && !item.IsDeleted);
            if (ret != null)
                return new StrategyDto
                {
                    Name = ret.Name,
                    Id = ret.Id,
                    UserId = ret.UserId,
                    Indicators = _indicatorsService.ConvertIndicators(ret.Indicators)
                };
            return new StrategyDto();
        }

        public void DeleteStrategy(int strategyId, int userId)
        {
            var strategy = _strategiesRepository.GetQueryable().FirstOrDefault(item => item.Id == strategyId && !item.IsDeleted);

            if(strategy == null)
                throw new BusinessException("Strategy not found", ErrorStatus.DataNotFound);

            if(strategy.UserId != userId)
                throw new BusinessException("You do not have permissions to this action", ErrorStatus.AccessDenied);

            strategy.IsDeleted = true;
            _strategiesRepository.Save();
        }

        public int CreateStrategy(StrategyDto strategy)
        {
            if (_strategiesRepository.GetQueryable().Any(s => s.UserId == strategy.UserId && s.Name == strategy.Name))
                throw new BusinessException(nameof(strategy.Name), "Strategy with this name already exists");
            if (!strategy.Indicators.Any())
                throw new BusinessException(nameof(strategy.Indicators), "At least one indicator has to be chosen");

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
