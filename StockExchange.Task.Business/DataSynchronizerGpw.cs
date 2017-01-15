using log4net;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using StockExchange.Common;
using StockExchange.DataAccess.Cache;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using StockExchange.Task.Business.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace StockExchange.Task.Business
{
    /// <summary>
    /// Synchronizes stock data using the GPW sources
    /// </summary>
    public sealed class DataSynchronizerGpw : IDataSynchronizerGpw
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<Price> _priceRepository;
        private readonly ICache _cache;

        /// <summary>
        /// Creates a new instance of <see cref="DataSynchronizerGpw"/>
        /// </summary>
        /// <param name="companyRepository"></param>
        /// <param name="priceRepository"></param>
        public DataSynchronizerGpw(IRepository<Company> companyRepository, IRepository<Price> priceRepository, ICache cache)
        {
            _companyRepository = companyRepository;
            _priceRepository = priceRepository;
            _cache = cache;
        }

        /// <inheritdoc />
        public async System.Threading.Tasks.Task Sync(DateTime date)
        {
            Logger.Debug("Syncing historical data started");

            var dateString = date.ToString(Consts.Formats.DateGpwFormat);
            IList<Company> companies = _companyRepository.GetQueryable().ToList();
            IList<Price> prices = _priceRepository.GetQueryable().Where(item => item.Date == date).ToList();
            var pricesToInsert = new List<Price>();
            string[,] data;
            try
            {
                data = LoadData(dateString);
            }
            catch (Exception ex)
            {
                Logger.Debug(date.ToString(CultureInfo.InvariantCulture) + ": " + ex.Message);
                return;
            }
            for (var i = 1; i < data.GetLength(0); ++i)
            {
                var day = DateTime.Parse(data[i, 0]);
                var name = data[i, 1].Trim();
                var open = decimal.Parse(data[i, 4].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture);
                var max = decimal.Parse(data[i, 5].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture);
                var min = decimal.Parse(data[i, 6].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture);
                var close = decimal.Parse(data[i, 7].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture);
                var volumen = int.Parse(data[i, 9]);
                if (companies.All(item => item.Code != name))
                {
                    _companyRepository.Insert(new Company
                    {
                        Code = name,
                        Name = ""
                    });
                    await _companyRepository.Save();
                    companies = _companyRepository.GetQueryable().ToList();
                }
                var company = companies.First(item => item.Code == name);
                if (prices.Any(item => item.CompanyId == company.Id))
                    continue;
                var price = new Price
                {
                    Date = day,
                    ClosePrice = close,
                    CompanyId = company.Id,
                    HighPrice = max,
                    LowPrice = min,
                    OpenPrice = open,
                    Volume = volumen
                };
                if (!PriceValidationHelper.IsInvalid(price))
                {
                    pricesToInsert.Add(price);
                }
            }
            _priceRepository.BulkInsert(pricesToInsert);
            await _priceRepository.Save();
            await _cache.Flush();

            Logger.Debug("Syncing historical data ended.");
        }

        private static string[,] LoadData(string dateString)
        {
            var url = CreatePathUrl(dateString);
            var fullPath = Path.GetTempFileName();
            using (var client = new WebClient())
            {
                client.DownloadFile(url, fullPath);
            }
            return ReadExcel(fullPath);
        }

        private static string[,] ReadExcel(string fullPath)
        {
            IWorkbook workbook;
            using (var file = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                var fs = new NPOIFSFileSystem(file);
                workbook = WorkbookFactory.Create(fs);
            }
            var sheet = workbook.GetSheetAt(0);
            var arr = new string[sheet.PhysicalNumberOfRows, sheet.GetRow(1).PhysicalNumberOfCells];
            for (var row = 0; row <= sheet.LastRowNum; row++)
            {
                for (var col = 0; col < sheet.GetRow(row).PhysicalNumberOfCells; col++)
                {
                    arr[row, col] = sheet.GetRow(row).GetCell(col).ToString();
                }
            }
            return arr;
        }

        private static string CreatePathUrl(string dateString)
        {
            return "https://www.gpw.pl/notowania_archiwalne?type=10&date=" + dateString + "&fetch.x=30&fetch.y=16";
        }
    }
}
