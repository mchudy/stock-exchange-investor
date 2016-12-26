using StockExchange.Business.Models.Indicators;
using System.Collections.Generic;
using StockExchange.Business.Indicators.Common;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface IIndicatorsService
    {
        IList<IndicatorProperty> GetPropertiesForIndicator(IndicatorType type);

        IList<CompanyIndicatorValues> GetIndicatorValues(IIndicator indicator, IList<int> companyIds);

        IList<CompanyIndicatorValues> GetIndicatorValues(IndicatorType type, IList<int> companyIds);

        IList<IndicatorType> GetAvailableIndicators();

        IList<IndicatorDto> GetIndicatorsForStrategy();

        IndicatorType? GetTypeFromName(string indicatorName);
    }
}