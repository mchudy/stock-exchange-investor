using StockExchange.DataAccess.Models;
using System;
using System.Linq.Expressions;

namespace StockExchange.Task.Business.Helpers
{
    internal static class PriceValidationHelper
    {
        internal static bool IsInvalid(Price p)
        {
            return IsInvalidExpression().Compile().Invoke(p);
        }

        internal static Expression<Func<Price, bool>> IsInvalidExpression()
        {
            return p => ((p.ClosePrice > 0 && p.LowPrice == 0 && p.HighPrice == 0 && p.OpenPrice == 0) ||
                          (p.OpenPrice < p.HighPrice && p.OpenPrice < p.LowPrice && p.OpenPrice == 0));
        }
    }
}
