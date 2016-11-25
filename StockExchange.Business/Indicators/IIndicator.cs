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
        IList<decimal> Calculate(IList<Price> historicalPrices);
    }

    /// <summary>
    /// Interfejs wskaźnika analizy technicznej rozszerzony o automatyczne pobieranie danych z bazy.
    /// </summary>
    public interface IAutoIndicator : IIndicator
    {
        IList<decimal> Calculate(DateTime startDate, DateTime endDate);
    }
}
