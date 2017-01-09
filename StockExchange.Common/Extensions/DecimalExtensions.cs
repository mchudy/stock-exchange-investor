using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace StockExchange.Common.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="decimal"/> class
    /// </summary>
    public static class DecimalExtension
    {
        private static readonly Dictionary<string, CultureInfo> ISOCurrenciesToACultureMap =
            CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(c => new { c, new RegionInfo(c.LCID).ISOCurrencySymbol })
                .GroupBy(x => x.ISOCurrencySymbol)
                .ToDictionary(g => g.Key, g => g.First().c, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Formats currency
        /// </summary>
        /// <param name="amount">The number to be formatted</param>
        /// <param name="currencyCode">The currency code</param>
        /// <returns>The formatted currency string</returns>
        public static string FormatCurrency(this decimal amount, string currencyCode = Consts.Formats.CurrencyCode)
        {
            CultureInfo culture;
            if (!ISOCurrenciesToACultureMap.TryGetValue(currencyCode, out culture)) return amount.ToString("0.00");
            CultureInfo customCulture = (CultureInfo)culture.Clone();
            customCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            return string.Format(customCulture, "{0:C}", amount);
        }
    }
}
