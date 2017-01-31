using log4net;
using StockExchange.Business.Exceptions;
using StockExchange.Business.Extensions;
using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Company;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Indicators;
using StockExchange.Business.Models.Paging;
using StockExchange.Business.Models.Price;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Common.Extensions;
using StockExchange.DataAccess.Cache;
using StockExchange.DataAccess.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StockExchange.Business.Services
{
    /// <summary>
    /// Provides operations on technical indicators
    /// </summary>
    public class IndicatorsService : IIndicatorsService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(IndicatorsService));

        private readonly IIndicatorFactory _indicatorFactory;
        private readonly IPriceService _priceService;
        private readonly ICompanyService _companyService;
        private readonly ICache _cache;

        /// <summary>
        /// Creates a new instance of <see cref="IndicatorsService"/>
        /// </summary>
        /// <param name="indicatorFactory"></param>
        /// <param name="priceService"></param>
        /// <param name="companyService"></param>
        /// <param name="cache"></param>
        public IndicatorsService(IIndicatorFactory indicatorFactory, IPriceService priceService,
            ICompanyService companyService, ICache cache)
        {
            _indicatorFactory = indicatorFactory;
            _priceService = priceService;
            _companyService = companyService;
            _cache = cache;
        }

        /// <inheritdoc />
        public IList<IndicatorType> GetAllIndicatorTypes()
        {
            return typeof(IndicatorType).GetEnumValues()
                .Cast<IndicatorType>()
                .ToList();
        }

        /// <inheritdoc />
        public IList<IndicatorDto> GetAllIndicators()
        {
            return typeof(IndicatorType).GetEnumValues().Cast<IndicatorType>().Select(i => new IndicatorDto
            {
                IndicatorType = i,
                IndicatorName = i.GetEnumDescription()
            }).ToList();
        }

        /// <inheritdoc/>
        public IList<IndicatorType> GetIndicatorTypesAvailableForStrategies()
        {
            var names = typeof(IndicatorType).GetFields()
                .Where(f => f.GetCustomAttribute<StrategyIgnoreIndicator>() == null)
                .Select(f => f.Name);
            return typeof(IndicatorType).GetEnumValues()
                .Cast<IndicatorType>()
                .Where(i => names.Contains(Enum.GetName(typeof(IndicatorType), i))).ToList();
        }

        /// <inheritdoc/>
        public IList<IndicatorDto> GetIndicatorsAvailableForStrategies()
        {
            return GetIndicatorTypesAvailableForStrategies().Select(x => new IndicatorDto()
            {
                IndicatorType = x,
                IndicatorName = x.GetEnumDescription()
            }).ToList();
        }

        /// <inheritdoc />
        public IndicatorType? GetTypeFromName(string indicatorName)
        {
            return typeof(IndicatorType).GetEnumValues()
                    .Cast<IndicatorType>()
                    .FirstOrDefault(i => i.ToString() == indicatorName);
        }

        /// <inheritdoc />
        public IndicatorDescriptionDto GetIndicatorDescription(IndicatorType indicatorType)
        {
            var indicator = _indicatorFactory.CreateIndicator(indicatorType);
            var attribute = indicator.GetType().GetCustomAttribute(typeof(IndicatorDescriptionAttribute)) as IndicatorDescriptionAttribute;
            return new IndicatorDescriptionDto()
            {
                IndicatorDescription = attribute?.IndicatorDescription,
                BuySignalDescription = attribute?.BuySignalDescription,
                SellSignalDescription = attribute?.SellSignalDescription
            };
        }

        /// <inheritdoc />
        public IList<IndicatorProperty> GetPropertiesForIndicator(IndicatorType type)
        {
            var indicator = _indicatorFactory.CreateIndicator(type);
            var properties = indicator.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => !p.GetCustomAttributes(typeof(IgnoreIndicatorProperty)).Any());
            return properties.Select(property => new IndicatorProperty
            {
                Name = property.Name,
                Value = (int)property.GetValue(indicator)
            }).ToList();
        }

        /// <inheritdoc />
        public async Task<IList<CompanyIndicatorValues>> GetIndicatorValues(IIndicator indicator, IList<int> companyIds)
        {
            IList<CompanyPricesDto> companyPrices = await _priceService.GetPrices(companyIds);
            return ComputeIndicatorValues(indicator, companyPrices);
        }

        /// <inheritdoc />
        public async Task<IList<CompanyIndicatorValues>> GetIndicatorValues(IndicatorType type, IList<int> companyIds, IList<IndicatorProperty> properties)
        {
            var propertiesDict = properties?.ToDictionary(t => t.Name, t => t.Value) ?? new Dictionary<string, int>();
            var indicator = _indicatorFactory.CreateIndicator(type, propertiesDict);
            return await GetIndicatorValues(indicator, companyIds);
        }

        /// <inheritdoc />
        public IList<ParameterizedIndicator> ConvertIndicators(IEnumerable<StrategyIndicator> indicators)
        {
            return indicators.Select(item => new ParameterizedIndicator
            {
                IndicatorType = (IndicatorType)item.IndicatorType,
                Properties = ConvertIndicatorProperties(item.Properties)
            }).ToList();
        }

        /// <inheritdoc />
        public async Task<IList<SignalEvent>> GetSignals(DateTime startDate, DateTime endDate, IList<int> companiesIds, IList<ParameterizedIndicator> indicators, bool isAnd, int daysLimitToAnd)
        {
            var signalEvents = new List<SignalEvent>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                signalEvents.Add(new SignalEvent
                {
                    Date = date,
                    CompaniesToBuy = new List<int>(),
                    CompaniesToSell = new List<int>()
                });
            }
            foreach (var company in companiesIds)
            {
                // przed pętlę
                var prices = await _priceService.GetPrices(company, endDate);
                if (!isAnd)
                {
                    CalculateSignalsForOrStrategy(endDate, indicators, prices, company, signalEvents);
                }
                else
                {
                    CalculateSignalsForAndStrategy(endDate, indicators, daysLimitToAnd, prices, company, signalEvents, startDate);
                }
            }
            signalEvents.RemoveAll(item => item.CompaniesToBuy.Count == 0 && item.CompaniesToSell.Count == 0);
            return signalEvents;
        }

        private void CalculateSignalsForAndStrategy(DateTime endDate, IList<ParameterizedIndicator> indicators, int daysLimitToAnd, IList<Price> prices,
            int company, List<SignalEvent> signalEvents, DateTime startDate)
        {
            IList<Signal> signals = new List<Signal>();
            foreach (var indicator in indicators)
            {
                if (indicator.IndicatorType == null)
                    continue;
                var ind = _indicatorFactory.CreateIndicator(indicator.IndicatorType.Value,
                    indicator.Properties.ToDictionary(t => t.Name, t => t.Value));
                try
                {
                    if (prices.Count <= ind.RequiredPricesForSignalCount)
                        continue;
                    var sig = ind.GenerateSignals(prices);
                    foreach (var signal in sig)
                    {
                        signals.Add(new Signal
                        {
                            Date = signal.Date,
                            Action = signal.Action,
                            IndicatorType = indicator.IndicatorType.Value
                        });
                    }
                }
                catch (Exception e) when (e is IndicatorArgumentException || e is ArgumentException)
                {
                    logger.Warn(
                        $"Error when generating signals for company {company} and indicator {ind.Type}, end date {endDate}", e);
                }
            }
            var signalsInScope = signals.Where(item => item.Date >= startDate).OrderBy(item => item.Date).ToList();
            foreach (var signal in signalsInScope)
            {
                var signaEvent = signalEvents.FirstOrDefault(item => item.Date == signal.Date);
                if (signaEvent == null) continue;
                if (signal.Action == SignalAction.Buy && !signaEvent.CompaniesToBuy.Contains(company))
                {
                    var signalsCount = GetSignalsCount(daysLimitToAnd, signalsInScope, signal, SignalAction.Buy);
                    if (signalsCount == indicators.Count)
                        signaEvent.CompaniesToBuy.Add(company);
                }
                if (signal.Action == SignalAction.Sell && !signaEvent.CompaniesToSell.Contains(company))
                {
                    var signalsCount = GetSignalsCount(daysLimitToAnd, signalsInScope, signal, SignalAction.Sell);
                    if (signalsCount == indicators.Count)
                        signaEvent.CompaniesToSell.Add(company);
                }
            }
        }

        private void CalculateSignalsForOrStrategy(DateTime endDate, IList<ParameterizedIndicator> indicators, IList<Price> prices, int company,
            List<SignalEvent> signalEvents)
        {
            foreach (var indicator in indicators)
            {
                if (indicator.IndicatorType == null)
                    continue;
                var ind = _indicatorFactory.CreateIndicator(indicator.IndicatorType.Value,
                    indicator.Properties.ToDictionary(t => t.Name, t => t.Value));
                IList<Signal> signals = new List<Signal>();
                try
                {
                    if (prices.Count < ind.RequiredPricesForSignalCount)
                        continue;
                    signals = ind.GenerateSignals(prices);
                }
                catch (Exception e) when (e is IndicatorArgumentException || e is ArgumentException)
                {
                    logger.Warn(
                        $"Error when generating signals for company {company} and indicator {ind.Type}, end date {endDate}", e);
                }
                foreach (var signal in signals)
                {
                    var signaEvent = signalEvents.FirstOrDefault(item => item.Date == signal.Date);
                    if (signaEvent == null) continue;
                    if (signal.Action == SignalAction.Buy && !signaEvent.CompaniesToBuy.Contains(company))
                        signaEvent.CompaniesToBuy.Add(company);
                    if (signal.Action == SignalAction.Sell && !signaEvent.CompaniesToSell.Contains(company))
                        signaEvent.CompaniesToSell.Add(company);
                }
            }
        }

        /// <inheritdoc />
        public async Task<PagedList<TodaySignal>> GetCurrentSignals(PagedFilterDefinition<TransactionFilter> message)
        {
            var allSignals = await _cache.Get<List<TodaySignal>>(CacheKeys.AllCurrentSignals) ??
                             await GetAllCurrentSignals();

            await _cache.Set(CacheKeys.AllCurrentSignals, allSignals);

            if (!string.IsNullOrWhiteSpace(message.Search))
                allSignals = allSignals.Where(item => item.Company.Contains(message.Search.ToUpper())).ToList();

            var first = true;
            if (message.OrderBys == null) return allSignals.ToPagedList(message.Start, message.Length);

            foreach (var orderBy in message.OrderBys)
            {
                if (!first) continue;
                if (orderBy.Column == "Indicator")
                    allSignals = orderBy.Desc ? allSignals.OrderByDescending(item => item.Indicator.Split(',').Length).ThenBy(item => item.Indicator).ThenBy(item => item.Company).ThenBy(item => item.Action).ToList()
                        : allSignals.OrderBy(item => item.Indicator.Split(',').Length).ThenBy(item => item.Indicator).ThenBy(item => item.Company).ThenBy(item => item.Action).ToList();
                if (orderBy.Column == "Company")
                    allSignals = orderBy.Desc ? allSignals.OrderByDescending(item => item.Company).ToList() : allSignals.OrderBy(item => item.Company).ToList();
                if (orderBy.Column == "Action")
                    allSignals = orderBy.Desc ? allSignals.OrderByDescending(item => item.Action).ThenByDescending(item => item.Indicator.Split(',').Length).ThenBy(item => item.Indicator).ThenBy(item => item.Company).ToList()
                        : allSignals.OrderBy(item => item.Action).ThenByDescending(item => item.Indicator.Split(',').Length).ThenBy(item => item.Indicator).ThenBy(item => item.Company).ToList();
                first = false;
            }

            return allSignals.ToPagedList(message.Start, message.Length);
        }

        /// <inheritdoc />
        public async Task<int> GetCurrentSignalsCount()
        {
            string cacheKey = CacheKeys.CurrentSignalsCount;
            var signalsCount = await _cache.Get<int?>(cacheKey);
            if (signalsCount.HasValue)
                return signalsCount.Value;

            var indicators = GetAllIndicators();
            var indicatorObjects = indicators.Select(indicator => _indicatorFactory.CreateIndicator(indicator.IndicatorType)).ToList();

            var prices = await _priceService.GetCurrentPrices(100);
            var companies = await _companyService.GetCompanies();
            var maxDate = await _priceService.GetMaxDate();

            var computed = 0;
            foreach (var company in companies)
            {
                var companyPrices = prices.Where(item => item.CompanyId == company.Id).OrderBy(item => item.Date).ToList();
                if (!companyPrices.Any()) continue;
                foreach (var indicator in indicatorObjects)
                {
                    IList<Signal> signals = new List<Signal>();
                    try
                    {
                        if (prices.Count < indicator.RequiredPricesForSignalCount)
                            continue;
                        signals = indicator.GenerateSignals(companyPrices);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn($"Error when generating signals for company {company.Code} and indicator {indicator.Type}", ex);
                    }
                    var todaySignal = signals.FirstOrDefault(item => item.Date == maxDate);
                    if (todaySignal != null)
                    {
                        computed++;
                    }
                }
            }
            await _cache.Set(cacheKey, computed);
            return computed;
        }

        private static int GetSignalsCount(int daysLimitToAnd, IList<Signal> signals, Signal signal, SignalAction action)
        {
            var scope =
                signals.Where(
                    item =>
                        item.Date <= signal.Date && item.Date >= signal.Date.AddDays(-daysLimitToAnd + 1) &&
                        item.Action == action).ToList();
            return scope
                .Select(item => item.IndicatorType)
                .Distinct()
                .Count();
        }

        private static List<TodaySignal> ComputeSignals(IList<CompanyDto> companies, IList<Price> prices, List<IIndicator> indicatorObjects, DateTime maxDate)
        {
            var computed = new List<TodaySignal>();
            foreach (var company in companies)
            {
                var companyPrices =
                    prices.Where(item => item.CompanyId == company.Id).OrderBy(item => item.Date).ToList();
                if (!companyPrices.Any()) continue;
                foreach (var indicator in indicatorObjects)
                {
                    IList<Signal> signals = new List<Signal>();
                    try
                    {
                        signals = indicator.GenerateSignals(companyPrices);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(
                            $"Error when generating signals for company {company.Code} and indicator {indicator.Type}",
                            ex);
                    }
                    var todaySignal = signals.FirstOrDefault(item => item.Date == maxDate);
                    if (todaySignal != null)
                    {
                        computed.Add(new TodaySignal
                        {
                            Action = todaySignal.Action.ToString(),
                            Company = company.Code,
                            Indicator = indicator.Type.ToString()
                        });
                    }
                }
            }
            return computed;
        }

        private async Task<List<TodaySignal>> GetAllCurrentSignals()
        {
            var indicators = GetAllIndicators();
            var indicatorObjects =
                indicators.Select(indicator => _indicatorFactory.CreateIndicator(indicator.IndicatorType)).ToList();

            var companies = await _companyService.GetCompanies();
            var maxDate = await _priceService.GetMaxDate();
            var prices = await _priceService.GetCurrentPrices(indicatorObjects.Max(i => i.RequiredPricesForSignalCount));

            var computed = ComputeSignals(companies, prices, indicatorObjects, maxDate);
            var ret = new List<TodaySignal>();
            ret.AddRange(computed.GroupBy(item => new
            {
                item.Company,
                item.Action
            })
                .Select(item => new TodaySignal
                {
                    Company = item.Key.Company,
                    Action = item.Key.Action,
                    Indicator = string.Join(", ", (IEnumerable<string>)item.Select(it => it.Indicator).ToArray())
                }));
            return ret.OrderBy(item => item.Company)
                .ThenBy(item => item.Action)
                .ThenBy(item => item.Indicator)
                .ToList();
        }

        private static IList<IndicatorProperty> ConvertIndicatorProperties(IEnumerable<StrategyIndicatorProperty> p)
        {
            return p.Select(item => new IndicatorProperty
            {
                Value = item.Value,
                Name = item.Name
            }).ToList();
        }

        private static IList<CompanyIndicatorValues> ComputeIndicatorValues(IIndicator indicator, IEnumerable<CompanyPricesDto> companyPrices)
        {
            return (companyPrices.Select(company => new { company, values = company.Prices.Count > indicator.RequiredPricesForSignalCount ? indicator.Calculate(company.Prices) : new List<IndicatorValue>() })
                .Select(@t => new CompanyIndicatorValues
                {
                    Company = @t.company.Company,
                    IndicatorValues = @t.values
                })).ToList();
        }
    }
}
