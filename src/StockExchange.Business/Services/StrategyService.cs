using StockExchange.Business.ErrorHandling;
using StockExchange.Business.Exceptions;
using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Indicators;
using StockExchange.Business.Models.Strategy;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.Business.Services
{
    /// <summary>
    /// Provides methods for operating on trading strategies
    /// </summary>
    public class StrategyService : IStrategyService
    {
        private readonly IStrategiesRepository _strategiesRepository;
        private readonly IIndicatorsService _indicatorsService;

        /// <summary>
        /// Creates a new instance of <see cref="StrategyService"/>
        /// </summary>
        /// <param name="strategiesRepository"></param>
        /// <param name="indicatorsService"></param>
        public StrategyService(IStrategiesRepository strategiesRepository, IIndicatorsService indicatorsService)
        {
            _strategiesRepository = strategiesRepository;
            _indicatorsService = indicatorsService;
        }

        /// <inheritdoc />
        public async Task<IList<StrategyDto>> GetStrategies(int userId)
        {
            var strategies =await _strategiesRepository.GetStrategies(userId);
            return strategies.Select(t => new StrategyDto
            {
                Id = t.Id,
                UserId = t.UserId,
                Name = t.Name,
                Indicators = t.Indicators.Select(i => new ParameterizedIndicator
                {
                    IndicatorType = (IndicatorType?) i.IndicatorType
                }).ToList(),
                SignalDaysPeriod = t.SignalDaysPeriod
            }).ToList();
        }

        /// <inheritdoc />
        public async Task<StrategyDto> GetStrategy(int userId, int strategyId)
        {
            var ret = await _strategiesRepository.GetStrategy(userId, strategyId);
            if (ret == null)
                throw new BusinessException($"Strategy not found", ErrorStatus.DataNotFound);

            return new StrategyDto
            {
                Name = ret.Name,
                Id = ret.Id,
                UserId = ret.UserId,
                SignalDaysPeriod = ret.SignalDaysPeriod,
                Indicators = _indicatorsService.ConvertIndicators(ret.Indicators)
            };
        }
        
        /// <inheritdoc />
        public async Task DeleteStrategy(int strategyId, int userId)
        {
            var strategy = await _strategiesRepository.GetStrategy(userId, strategyId);
            if(strategy == null)
                throw new BusinessException("Strategy not found", ErrorStatus.DataNotFound);
            if(strategy.UserId != userId)
                throw new BusinessException("You do not have permissions to this action", ErrorStatus.AccessDenied);
            strategy.IsDeleted = true;
            await _strategiesRepository.Save();
        }

        /// <inheritdoc />
        public async Task UpdateStrategy(StrategyDto dto)
        {
            var strategy = await _strategiesRepository.GetStrategy(dto.UserId, dto.Id);
            if(strategy == null)
                throw new BusinessException("Strategy not found", ErrorStatus.DataNotFound);

            strategy.Name = dto.Name;
            strategy.SignalDaysPeriod = dto.IsConjunctiveStrategy ? dto.SignalDaysPeriod : null;

            var toDelete = strategy.Indicators
                .Where(i => dto.Indicators
                .All(im => im.IndicatorType != (IndicatorType) i.IndicatorType))
                .ToList();
            foreach (var strategyIndicator in toDelete)
            {
                _strategiesRepository.DeleteIndicator(strategyIndicator);
            }
            foreach (var indicatorDto in dto.Indicators)
            {
                var indicator = strategy.Indicators.FirstOrDefault(i => (IndicatorType)i.IndicatorType == indicatorDto.IndicatorType);
                if(indicator == null)
                    AddIndicator(indicatorDto, strategy);
                else
                {
                    foreach (var property in indicatorDto.Properties)
                    {
                        var indicatorProperty = indicator.Properties.FirstOrDefault(p => p.Name == property.Name);
                        if(indicatorProperty == null)
                            continue;
                        indicatorProperty.Value = property.Value;
                    }
                }
            }
            await _strategiesRepository.Save();
        }

        /// <inheritdoc />
        public async Task<int> CreateStrategy(StrategyDto strategy)
        {
            if (await _strategiesRepository.StrategyExists(strategy.UserId, strategy.Name))
                throw new BusinessException(nameof(strategy.Name), "Strategy with this name already exists");
            if (!strategy.Indicators.Any())
                throw new BusinessException(nameof(strategy.Indicators), "At least one indicator has to be chosen");
            var investmentStrategy = new InvestmentStrategy
            {
                UserId = strategy.UserId,
                Name = strategy.Name,
                SignalDaysPeriod = strategy.SignalDaysPeriod,
                Indicators = new List<StrategyIndicator>()
            };
            foreach (var indicator in strategy.Indicators)
            {
                AddIndicator(indicator, investmentStrategy);
            }
            _strategiesRepository.Insert(investmentStrategy);
            await _strategiesRepository.Save();
            return investmentStrategy.Id;
        }

        private void AddIndicator(ParameterizedIndicator indicator, InvestmentStrategy investmentStrategy)
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
    }
}
