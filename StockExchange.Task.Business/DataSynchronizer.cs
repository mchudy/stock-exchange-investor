using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using StockExchange.Common;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using StockExchange.Task.Business.Helpers;

namespace StockExchange.Task.Business
{
    public sealed class DataSynchronizer : IDataSynchronizer
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<Price> _priceRepository; 

        public DataSynchronizer(IRepository<Company> companyRepository, IRepository<Price> priceRepository)
        {
            _companyRepository = companyRepository;
            _priceRepository = priceRepository;
        }

        public void Sync(DateTime startDate, DateTime endDate, IEnumerable<string> companyCodes = null)
        {
            Logger.Debug("Syncing historical data started");
            var startDateString = startDate.ToString(Consts.Formats.DateFormat);
            var endDateString = endDate.ToString(Consts.Formats.DateFormat);
            if (companyCodes == null)
            {
                companyCodes = _companyRepository.GetQueryable().Select(item => item.code);
            }
            foreach (var url in companyCodes.Select(companyCode => CreatePathUrl(startDateString, endDateString, companyCode)))
            {
                SyncByCompany(url);
            }
            Logger.Debug("Syncing historical data ended.");
        }

        private static void SyncByCompany(string url)
        {
            var data = CsvImporter.GetCsv(url);
            data.RemoveAt(0);
            foreach (var row in data)
            {
                // TODO
            }
        }

        private static string CreatePathUrl(string startDateString, string endDateString, string companyCode)
        {
            return "http://stooq.pl/q/d/l/?s=" + companyCode + "&d1=" + startDateString + "&d2=" + endDateString + "&i=d";
        }
    }
}
