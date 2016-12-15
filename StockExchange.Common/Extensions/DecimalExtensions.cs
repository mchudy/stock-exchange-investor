using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace StockExchange.Common.Extensions
{
    public static class DecimalExtension
    {
        private static readonly Dictionary<string, CultureInfo> ISOCurrenciesToACultureMap =
            CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(c => new { c, new RegionInfo(c.LCID).ISOCurrencySymbol })
                .GroupBy(x => x.ISOCurrencySymbol)
                .ToDictionary(g => g.Key, g => g.First().c, StringComparer.OrdinalIgnoreCase);

        public static string FormatCurrency(this decimal amount, string currencyCode = Consts.Formats.CurrencyCode)
        {
            CultureInfo culture;
            if (ISOCurrenciesToACultureMap.TryGetValue(currencyCode, out culture))
            {
                CultureInfo customCulture = (CultureInfo) culture.Clone();
                customCulture.NumberFormat.CurrencyDecimalSeparator = ".";
                return string.Format(customCulture, "{0:C}", amount);
            }
            return amount.ToString("0.00");
        }
    }
}
