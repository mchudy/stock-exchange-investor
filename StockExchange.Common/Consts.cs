namespace StockExchange.Common
{
    /// <summary>
    /// Global project constants
    /// </summary>
    public static class Consts
    {
        /// <summary>
        /// Commands for the Task consola app
        /// </summary>
        public static class Commands
        {
            /// <summary>
            /// The help command
            /// </summary>
            public const string Help = "help";

            /// <summary>
            /// The command synchronizing stock data
            /// </summary>
            public const string SyncData = "sync-data";

            /// <summary>
            /// The command synchronizing stock data from GPW sources
            /// </summary>
            public const string SyncDataGpw = "sync-data-gpw";

            /// <summary>
            /// The command synchronizing stock data from today
            /// </summary>
            public const string SyncTodayData = "sync-today-data";

            /// <summary>
            /// The command synchronizing stock data from today from GPW sources
            /// </summary>
            public const string SyncTodayDataGpw = "sync-today-data-gpw";

            /// <summary>
            /// The command for fixing data from the GPW
            /// </summary>
            public const string FixData = "fix-data";
        }

        /// <summary>
        /// Global formats
        /// </summary>
        public static class Formats
        {
            /// <summary>
            /// Date format used when synchronizing the data
            /// </summary>
            public const string DateFormat = "yyyyMMdd";

            /// <summary>
            /// Date format used by the GPW site
            /// </summary>
            public const string DateGpwFormat = "yyyy-MM-dd";

            /// <summary>
            /// Default currency format
            /// </summary>
            public const string Currency = "{0:#,##0.00}";

            /// <summary>
            /// Default integer format
            /// </summary>
            public const string Integer = "{0:###0}";

            /// <summary>
            /// Default date format
            /// </summary>
            public const string DisplayDate = "{0:dd/MM/yyyy}";

            /// <summary>
            /// Default currency code
            /// </summary>
            public const string CurrencyCode = "PLN";
        }

        /// <summary>
        /// Default parameter values for syncing stock data commands
        /// </summary>
        public static class SyncDataParameters
        {
            /// <summary>
            /// Default start date
            /// </summary>
            public const string StartDate = "2006/01/01";
        }
    }
}
