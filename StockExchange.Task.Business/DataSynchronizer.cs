using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IFactory<IRepository<Company>> _companyRepositoryFactory;
        private readonly IFactory<IRepository<Price>> _priceRepositoryFactory;

        public DataSynchronizer(IFactory<IRepository<Company>> companyRepositoryFactory, IFactory<IRepository<Price>> priceRepositoryFactory)
        {
            _companyRepositoryFactory = companyRepositoryFactory;
            _priceRepositoryFactory = priceRepositoryFactory;
        }

        public void Sync(DateTime startDate, DateTime endDate, IEnumerable<string> companyCodes = null)
        {
            Logger.Debug("Syncing historical data started");
            var startDateString = startDate.ToString(Consts.Formats.DateFormat);
            var endDateString = endDate.ToString(Consts.Formats.DateFormat);
            if (companyCodes == null)
            {
                companyCodes = _companyRepositoryFactory.CreateInstance().GetQueryable().Select(item => item.code);
            }
            var enumerable = companyCodes as IList<string> ?? companyCodes.ToList();
            Parallel.ForEach(enumerable, companyCode => ThreadSync(startDateString, endDateString, companyCode));
            Logger.Debug("Syncing historical data ended.");
        }

        private void ThreadSync(string startDateString, string endDateString, string companyCode)
        {
            var url = CreatePathUrl(startDateString, endDateString, companyCode);
            SyncByCompany(url, companyCode);
        }

        private void SyncByCompany(string url, string companyCode)
        {
            var data = CsvImporter.GetCsv(url);
            data.RemoveAt(0);
            using (var companyRepository = _companyRepositoryFactory.CreateInstance())
            {
                using (var priceRepository = _priceRepositoryFactory.CreateInstance())
                {
                    foreach (var row in data)
                    {
                        var company = companyRepository.GetQueryable(item => item.code == companyCode).FirstOrDefault() ?? new Company { id = 0 };
                        var currentDate = DateTime.Parse(row[0]);
                        if (!priceRepository.GetQueryable(item => item.companyId == company.id && item.date == currentDate).Any())
                        {
                            priceRepository.Insert(PriceConverter.Convert(row, company));
                        }
                    }
                    priceRepository.Save();
                }
            }

        }

        private static string CreatePathUrl(string startDateString, string endDateString, string companyCode)
        {
            return "http://stooq.pl/q/d/l/?s=" + companyCode + "&d1=" + startDateString + "&d2=" + endDateString + "&i=d";
        }
    }
}
