using System;
using System.Collections.Generic;
using System.Globalization;
using StockExchange.DataAccess.Models;

namespace StockExchange.Task.Business.Helpers
{
    internal static class PriceConverter
    {
        internal static Price Convert(IList<string> data, Company company)
        {
            return new Price
            {
                Date = DateTime.Parse(data[0]),
                CompanyId = company.Id,
                OpenPrice = decimal.Parse(data[1], NumberStyles.Any, CultureInfo.InvariantCulture),
                HighPrice = decimal.Parse(data[2], NumberStyles.Any, CultureInfo.InvariantCulture),
                LowPrice = decimal.Parse(data[3], NumberStyles.Any, CultureInfo.InvariantCulture),
                ClosePrice = decimal.Parse(data[4], NumberStyles.Any, CultureInfo.InvariantCulture),
                Volume = int.Parse(data[5])
            };
        }
    }
}
