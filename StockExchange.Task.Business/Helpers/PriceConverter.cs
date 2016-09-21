using System;
using System.Collections.Generic;
using System.Globalization;
using StockExchange.DataAccess.Models;

namespace StockExchange.Task.Business.Helpers
{
    public static class PriceConverter
    {
        public static Price Convert(IList<string> data, Company company)
        {
            return new Price
            {
                date = DateTime.Parse(data[0]),
                companyId = company.id,
                openPrice = decimal.Parse(data[1], NumberStyles.Any, CultureInfo.InvariantCulture),
                highPrice = decimal.Parse(data[2], NumberStyles.Any, CultureInfo.InvariantCulture),
                lowPrice = decimal.Parse(data[3], NumberStyles.Any, CultureInfo.InvariantCulture),
                closePrice = decimal.Parse(data[4], NumberStyles.Any, CultureInfo.InvariantCulture),
                volume = int.Parse(data[5])
            };
        }
    }
}
