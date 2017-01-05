using System;
using StockExchange.Business.Models.Indicators;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockExchange.Business.Extensions;
using StockExchange.Business.Indicators.Common;
using StockExchange.Business.Models.Filters;
using StockExchange.DataAccess.Models;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface IIndicatorsService
    {
        IList<IndicatorProperty> GetPropertiesForIndicator(IndicatorType type);

        Task<IList<CompanyIndicatorValues>> GetIndicatorValues(IIndicator indicator, IList<int> companyIds);

        Task<IList<CompanyIndicatorValues>> GetIndicatorValues(IndicatorType type, IList<int> companyIds, IList<IndicatorProperty> properties);

        IList<IndicatorType> GetAllIndicatorTypes();

        IList<IndicatorDto> GetAllIndicators();

        IndicatorType? GetTypeFromName(string indicatorName);

        IList<ParameterizedIndicator> ConvertIndicators(IEnumerable<StrategyIndicator> indicators);

        Task<IList<SignalEvent>> GetSignals(DateTime startDate, DateTime endDate, IList<int> companiesIds, IList<ParameterizedIndicator> indicators);

        Task<PagedList<TodaySignal>> GetSignals(PagedFilterDefinition<TransactionFilter> message);

        Task<int> GetSignalsCount();
    }
}