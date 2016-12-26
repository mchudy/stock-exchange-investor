using System.Collections.Generic;
using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;

namespace StockExchange.Business.Indicators.Common
{
    /// <summary>
    /// Interfejs wskaźnika analizy technicznej.
    /// </summary>
    public interface IIndicator
    {
        IndicatorType Type { get; }

        IList<IndicatorValue> Calculate(IList<Price> prices);

        IList<Signal> GenerateSignals(IList<IndicatorValue> values);
    }
}
