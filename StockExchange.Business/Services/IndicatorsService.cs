using log4net;
using StockExchange.Business.Extensions;
using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Indicators;
using StockExchange.Business.Models.Price;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Common.Extensions;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StockExchange.Business.Services
{
    public class IndicatorsService : IIndicatorsService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(IndicatorsService));

        private readonly IIndicatorFactory _indicatorFactory;
        private readonly IPriceService _priceService;
        private readonly ICompanyService _companyService;

        public IndicatorsService(IIndicatorFactory indicatorFactory, IPriceService priceService, ICompanyService companyService)
        {
            _indicatorFactory = indicatorFactory;
            _priceService = priceService;
            _companyService = companyService;
        }

        public IList<IndicatorType> GetAllIndicatorTypes()
        {
            return typeof(IndicatorType).GetEnumValues()
                .Cast<IndicatorType>()
                .ToList();
        }

        public IList<IndicatorDto> GetAllIndicators()
        {
            return typeof(IndicatorType).GetEnumValues().Cast<IndicatorType>().Select(i => new IndicatorDto
            {
                IndicatorType = i,
                IndicatorName = i.GetEnumDescription()
            }).ToList();
        }

        public IndicatorType? GetTypeFromName(string indicatorName)
        {
            return typeof(IndicatorType).GetEnumValues()
                    .Cast<IndicatorType>()
                    .FirstOrDefault(i => i.ToString() == indicatorName);
        }

        public IList<IndicatorProperty> GetPropertiesForIndicator(IndicatorType type)
        {
            var indicator = _indicatorFactory.CreateIndicator(type);
            var properties = indicator.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.Name != nameof(IIndicator.Type));
            return properties.Select(property => new IndicatorProperty
            {
                Name = property.Name,
                Value = (int)property.GetValue(indicator)
            }).ToList();
        }

        public IList<CompanyIndicatorValues> GetIndicatorValues(IIndicator indicator, IList<int> companyIds)
        {
            IList<CompanyPricesDto> companyPrices = _priceService.GetPricesForCompanies(companyIds);
            return ComputeIndicatorValues(indicator, companyPrices);
        }

        public IList<CompanyIndicatorValues> GetIndicatorValues(IndicatorType type, IList<int> companyIds, IList<IndicatorProperty> properties)
        {
            var propertiesDict = properties?.ToDictionary(t => t.Name, t => t.Value) ?? new Dictionary<string, int>();
            var indicator = _indicatorFactory.CreateIndicator(type, propertiesDict);
            return GetIndicatorValues(indicator, companyIds);
        }

        public IList<ParameterizedIndicator> ConvertIndicators(IEnumerable<StrategyIndicator> indicators)
        {
            return indicators.Select(item => new ParameterizedIndicator
            {
                IndicatorType = (IndicatorType)item.IndicatorType,
                Properties = ConvertIndicatorProperties(item.Properties)
            }).ToList();
        }

        public IList<SignalEvent> GetSignals(DateTime startDate, DateTime endDate, IList<int> companiesIds, IList<ParameterizedIndicator> indicators)
        {
            var signalEvents = new List<SignalEvent>();
            for (var date = startDate; date < endDate; date = date.AddDays(1))
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
                var prices = _priceService.GetPrices(company, endDate);
                foreach (var indicator in indicators)
                {
                    if (indicator.IndicatorType == null) continue;
                    var ind = _indicatorFactory.CreateIndicator(indicator.IndicatorType.Value, indicator.Properties.ToDictionary(t => t.Name, t => t.Value));
                    IList<Signal> signals = new List<Signal>();
                    try
                    {
                        signals = ind.GenerateSignals(prices);
                    }
                    catch (ArgumentException e)
                    {
                        logger.Warn($"Error when generating signals for company {company} and indicator {ind.Type}, end date {endDate}", e);
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
            signalEvents.RemoveAll(item => item.CompaniesToBuy.Count == 0 && item.CompaniesToSell.Count == 0);
            return signalEvents;
        }

        public PagedList<TodaySignal> GetSignals(PagedFilterDefinition<TransactionFilter> message)
        {
            var indicators = GetAllIndicators();
            var indicatorObjects = indicators.Select(indicator => _indicatorFactory.CreateIndicator(indicator.IndicatorType)).ToList();
            var prices = _priceService.GetLastPricesForAllCompanies();
            var companies = _companyService.GetAllCompanies();
            var maxDate = _priceService.GetMaxDate();
            var computed = new List<TodaySignal>();
            foreach (var company in companies)
            {
                var companyPrices = prices.Where(item => item.CompanyId == company.Id).OrderBy(item => item.Date).ToList();
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
                        logger.Warn($"Error when generating signals for company {company} and indicator {indicator.Type}", ex);
                    }
                    var todaySignal = signals.FirstOrDefault(item => item.Date == maxDate);
                    if (todaySignal != null)
                    {
                        computed.Add(new TodaySignal { Action = todaySignal.Action.ToString(), Company = company.Code, Indicator = indicator.Type.ToString() });
                    }
                }
            }
            var ret = new List<TodaySignal>();
            ret.AddRange(computed.GroupBy(item => new { item.Company, item.Action }).Select(item => new TodaySignal { Company = item.Key.Company, Action = item.Key.Action, Indicator = string.Join(", ", (IEnumerable<string>)item.Select(it => it.Indicator).ToArray())}));
            return ret.OrderBy(item => item.Company).ThenBy(item => item.Action).ThenBy(item => item.Indicator).ToPagedList(message.Start, message.Length);
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
            return (from company in companyPrices
                    let values = indicator.Calculate(company.Prices)
                    select new CompanyIndicatorValues
                    {
                        Company = company.Company,
                        IndicatorValues = values
                    }).ToList();
        }
    }
}
