using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using StockExchange.Business.Models;

namespace StockExchange.Business.Indicators
{
    /// <summary>
    /// Interfejs wskaźnika analizy technicznej.
    /// </summary>
    public interface IIndicator
    {
        IList<IndicatorValue> Calculate(IList<Price> prices);
    }
}
