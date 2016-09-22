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
        private readonly IRepository<Company> _companyRepository;
        private readonly IFactory<IRepository<Price>> _priceRepositoryFactory;

        public DataSynchronizer(IRepository<Company> companyRepository, IFactory<IRepository<Price>> priceRepositoryFactory)
        {
            _companyRepository = companyRepository;
            _priceRepositoryFactory = priceRepositoryFactory;
        }

        public void Sync(DateTime startDate, DateTime endDate, IEnumerable<string> companyCodes = null)
        {
            Logger.Debug("Syncing historical data started");
            var startDateString = startDate.ToString(Consts.Formats.DateFormat);
            var endDateString = endDate.ToString(Consts.Formats.DateFormat);
            IList<Company> companies = companyCodes == null ? _companyRepository.GetQueryable().ToList() : _companyRepository.GetQueryable(item => companyCodes.ToList().Contains(item.code)).ToList();
            IList<Price> prices = _priceRepositoryFactory.CreateInstance().GetQueryable().ToList();
            Parallel.ForEach(companies, company => ThreadSync(startDateString, endDateString, company, prices));
            Logger.Debug("Syncing historical data ended.");
        }

        private void ThreadSync(string startDateString, string endDateString, Company company, IList<Price> prices)
        {
            var url = CreatePathUrl(startDateString, endDateString, company.code);
            SyncByCompany(url, company, prices);
        }

        private void SyncByCompany(string url, Company company, IList<Price> prices)
        {
            var data = CsvImporter.GetCsv(url);
            data.RemoveAt(0);
            using (var priceRepository = _priceRepositoryFactory.CreateInstance())
            {
                var inserted = false;
                // ReSharper disable once LoopCanBePartlyConvertedToQuery
                foreach (var row in data)
                {
                    var currentDate = DateTime.Parse(row[0]);
                    // ReSharper disable once InvertIf
                    if (!prices.Any(item => item.companyId == company.id && item.date == currentDate))
                    {
                        priceRepository.Insert(PriceConverter.Convert(row, company));
                        inserted = true;
                    }
                }
                if (inserted)
                {
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
