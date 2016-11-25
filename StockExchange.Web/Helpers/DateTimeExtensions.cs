using System;

namespace StockExchange.Web.Helpers
{
    public static class DateTimeExtensions
    {
        // TODO: Move to Consts
        private static readonly long DatetimeMinTimeTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

        public static long ToJavaScriptTimeStamp(this DateTime dt)
        {
            return (TimeZoneInfo.ConvertTimeToUtc(dt).ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 10000;
        }
    }
}