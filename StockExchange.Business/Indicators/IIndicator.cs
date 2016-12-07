using StockExchange.Business.Models;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using StockExchange.Business.Models.Indicators;

namespace StockExchange.Business.Indicators
{
    /// <summary>
    /// Interfejs wskaźnika analizy technicznej.
    /// </summary>
    public interface IIndicator
    {
        IndicatorType Type { get; }

        IList<IndicatorValue> Calculate(IList<Price> prices);
    }
}
