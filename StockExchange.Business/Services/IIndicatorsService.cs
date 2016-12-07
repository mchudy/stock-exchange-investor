using StockExchange.Business.Indicators;
using StockExchange.Business.Models.Indicators;
using System.Collections.Generic;

namespace StockExchange.Business.Services
{
    public interface IIndicatorsService
    {
        IList<IndicatorProperty> GetPropertiesForIndicator(IndicatorType type);
    }
}