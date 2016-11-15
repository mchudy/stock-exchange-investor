using log4net;
using StockExchange.Common;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using StockExchange.Task.Business.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.Task.Business
{
    public sealed class DataSynchronizerGpw : IDataSynchronizerGpw
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IRepository<Company> _companyRepository;
        private readonly IFactory<IRepository<Price>> _priceRepositoryFactory;

        public DataSynchronizerGpw(IRepository<Company> companyRepository, IFactory<IRepository<Price>> priceRepositoryFactory)
        {
            _companyRepository = companyRepository;
            _priceRepositoryFactory = priceRepositoryFactory;
        }

        public void Sync(DateTime date)
        {
            Logger.Debug("Syncing historical data started");
            var dateString = date.ToString(Consts.Formats.DateGpwFormat);
            IList<Company> companies = _companyRepository.GetQueryable().ToList();
            IList<Price> prices = _priceRepositoryFactory.CreateInstance().GetQueryable().ToList();
            var url = CreatePathUrl(dateString);

            Logger.Debug("Syncing historical data ended.");
        }

        private void SyncByCompany(string url, Company company, IList<Price> prices)
        {
            var data = CsvImporter.GetCsv(url);
            data.RemoveAt(0);
            if (!data.Any())
            {
                Logger.Warn($"No data available for company {company.Code}");
            }
            using (var priceRepository = _priceRepositoryFactory.CreateInstance())
            {
                var inserted = false;
                // ReSharper disable once LoopCanBePartlyConvertedToQuery
                foreach (var row in data)
                {
                    var currentDate = DateTime.Parse(row[0]);
                    // ReSharper disable once InvertIf
                    if (!prices.Any(item => item.CompanyId == company.Id && item.Date == currentDate))
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

        private static string CreatePathUrl(string dateString)
        {
            return "https://www.gpw.pl/notowania_archiwalne?type=10&date=" + dateString + "&fetch.x=30&fetch.y=16";
        }
    }
}
