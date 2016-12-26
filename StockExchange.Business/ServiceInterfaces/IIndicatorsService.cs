using StockExchange.Business.Models.Indicators;
using System.Collections.Generic;
using StockExchange.Business.Indicators.Common;
using StockExchange.DataAccess.Models;

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

        IList<ParameterizedIndicator> ConvertIndicators(IEnumerable<StrategyIndicator> i);

        IList<Signal> GetIndicatorSignals(IList<IndicatorValue> values, IndicatorType type);
    }
}