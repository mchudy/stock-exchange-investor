namespace StockExchange.Common
{
    public static class Consts
    {
        public static class Commands
        {
            public const string Help = "help";
            public const string SyncData = "sync-data";
            public const string SyncDataGpw = "sync-data-gpw";
            public const string SyncTodayData = "sync-today-data";
            public const string SyncTodayDataGpw = "sync-today-data-gpw";
        }

        public static class Formats
        {
            public const string DateFormat = "yyyyMMdd";
            public const string DateGpwFormat = "yyyy-MM-dd";
            public const string Currency = "{0:#,##0.00}";
            public const string Integer = "{0:#,##0}";
            public const string DisplayDate = "{0:dd/MM/yyyy}";
        }

        public static class SyncDataParameters
        {
            public const string StartDate = "2006/01/01";
        }
    }
}
