using System;
using log4net;
using StockExchange.Task.Business.Helpers;

namespace StockExchange.Task.Business
{
    public sealed class HistoricalDataSynchronizer : IHistoricalDataSynchronizer
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Sync(DateTime startDate, DateTime endDate, string companyCode = null)
        {
            Logger.Debug("Syncing historical data started");
            var startDateString = startDate.ToString("yyyyMMdd");
            var endDateString = endDate.ToString("yyyyMMdd");
            if (companyCode == null)
            {
                companyCode = "kgh";
            }
            var url = "http://stooq.pl/q/d/l/?s=" + companyCode + "&d1=" + startDateString + "&d2=" + endDateString + "&i=d";
            var data = CsvImporter.GetCsv(url);
            data.RemoveAt(0);
            foreach (var row in data)
            {
                
            }
            Logger.Debug("Syncing historical data ended.");
        }
    }
}
