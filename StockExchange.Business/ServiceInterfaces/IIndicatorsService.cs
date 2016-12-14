using StockExchange.Business.Indicators;
using StockExchange.Business.Models.Indicators;
using System.Collections.Generic;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface IIndicatorsService
    {
        IList<IndicatorProperty> GetPropertiesForIndicator(IndicatorType type);

        IList<CompanyIndicatorValues> GetIndicatorValues(IIndicator indicator, IList<int> companyIds);
        IList<CompanyIndicatorValues> GetIndicatorValues(IndicatorType type, IList<int> companyIds);

        IList<IndicatorType> GetAvailableIndicators();
    }
}