using StockExchange.Business.Models.Indicators;
using StockExchange.Business.ServiceInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Price;
using StockExchange.Common.Extensions;
using StockExchange.DataAccess.Models;

namespace StockExchange.Business.Services
{
    public class IndicatorsService : IIndicatorsService
    {
        private readonly IIndicatorFactory _indicatorFactory;
        private readonly IPriceService _priceService;

        public IndicatorsService(IIndicatorFactory indicatorFactory, IPriceService priceService)
        {
            _indicatorFactory = indicatorFactory;
            _priceService = priceService;
        }

        public IList<IndicatorType> GetAvailableIndicators()
        {
            return typeof(IndicatorType).GetEnumValues()
                .Cast<IndicatorType>()
                .ToList();
        }

        public IList<IndicatorDto> GetIndicatorsForStrategy()
        {
            return typeof(IndicatorType).GetEnumValues().Cast<IndicatorType>().Select(i => new IndicatorDto()
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

        public IList<CompanyIndicatorValues> GetIndicatorValues(IndicatorType type, IList<int> companyIds)
        {
            var indicator = _indicatorFactory.CreateIndicator(type);
            return GetIndicatorValues(indicator, companyIds);
        }

        public IList<ParameterizedIndicator> ConvertIndicators(IEnumerable<StrategyIndicator> i)
        {
            return i.Select(item => new ParameterizedIndicator
            {
                IndicatorType = (IndicatorType)item.IndicatorType,
                Properties = ConvertIndicatorProperties(item.Properties)
            }).ToList();
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
                    Company = company.Company, IndicatorValues = values
                }).ToList();
        }
    }
}
