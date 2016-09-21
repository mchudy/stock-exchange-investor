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
            foreach (var companyCode in companyCodes)
            {
                var url = CreatePathUrl(startDateString, endDateString, companyCode);
                SyncByCompany(url, companyCode);
            }
            Logger.Debug("Syncing historical data ended.");
        }

        private void SyncByCompany(string url, string companyCode)
        {
            var data = CsvImporter.GetCsv(url);
            data.RemoveAt(0);
            foreach (var row in data)
            {
                var company = _companyRepository.GetQueryable(item => item.code == companyCode).FirstOrDefault() ?? new Company { id = 0 };
                var currentDate = DateTime.Parse(row[0]);
                if (!_priceRepository.GetQueryable(item => item.companyId == company.id && item.date == currentDate).Any())
                {
                    _priceRepository.Insert(PriceConverter.Convert(row, company));
                }
            }
            _priceRepository.Save();
        }

        private static string CreatePathUrl(string startDateString, string endDateString, string companyCode)
        {
            return "http://stooq.pl/q/d/l/?s=" + companyCode + "&d1=" + startDateString + "&d2=" + endDateString + "&i=d";
        }
    }
}
