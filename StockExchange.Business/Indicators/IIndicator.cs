using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;

namespace StockExchange.Business.Indicators
{
    /// <summary>
    /// Interfejs wskaźnika analizy technicznej.
    /// </summary>
    public interface IIndicator
    {
        IList<IndicatorValue> Calculate(IList<Price> historicalPrices);
    }
}
