using log4net;

namespace StockExchange.Task.Business
{
    public sealed class HistoricalDataSynchronizer : IHistoricalDataSynchronizer
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Sync()
        {
            Logger.Debug("Syncing historical data started");
            // TODO
            Logger.Debug("Syncing historical data ended.");
        }
    }
}
