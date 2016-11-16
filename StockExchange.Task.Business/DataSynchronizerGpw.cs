using log4net;
using StockExchange.Common;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace StockExchange.Task.Business
{
    public sealed class DataSynchronizerGpw : IDataSynchronizerGpw
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<Price> _priceRepository;

        public DataSynchronizerGpw(IRepository<Company> companyRepository, IRepository<Price> priceRepository)
        {
            _companyRepository = companyRepository;
            _priceRepository = priceRepository;
        }

        public void Sync(DateTime date)
        {
            Logger.Debug("Syncing historical data started");
            var dateString = date.ToString(Consts.Formats.DateGpwFormat);
            IList<Company> companies = _companyRepository.GetQueryable().ToList();
            IList<Price> prices = _priceRepository.GetQueryable().ToList();
            var url = CreatePathUrl(dateString);
            var client = new WebClient();
            var fullPath = Path.GetTempFileName();
            client.DownloadFile(url, fullPath);
            var data = ReadExcel(fullPath);
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
                    _companyRepository.Save();
                    companies = _companyRepository.GetQueryable().ToList();
                }
                var company = companies.First(item => item.Code == name);
                if (prices.Any(item => item.Date == day && item.CompanyId == company.Id)) continue;
                _priceRepository.Insert(new Price
                {
                    Date = day,
                    ClosePrice = close,
                    CompanyId = company.Id,
                    HighPrice = max,
                    LowPrice = min,
                    OpenPrice = open,
                    Volume = volumen
                });            
            }
            _priceRepository.Save();
            Logger.Debug("Syncing historical data ended.");
        }

        private static string[,] ReadExcel(string fullPath)
        {
            // Reference to Excel Application
            var xlApp = new Excel.Application();
            var xlWorkbook = xlApp.Workbooks.Open(fullPath);
            // Get the first worksheet
            var xlWorksheet = (Excel.Worksheet)xlWorkbook.Sheets.Item[1];
            // Get the range of cells which has data.
            var xlRange = xlWorksheet.UsedRange;
            // Get an object array of all of the cells in the worksheet with their values
            var valueArray = (object[,])xlRange.Value[Excel.XlRangeValueDataType.xlRangeValueDefault];
            // iterate through each cell and display the contents
            var arr = new string[xlWorksheet.UsedRange.Rows.Count, xlWorksheet.UsedRange.Columns.Count];
            for (var row = 1; row <= xlWorksheet.UsedRange.Rows.Count; ++row)
            {
                for (var col = 1; col <= xlWorksheet.UsedRange.Columns.Count; ++col)
                {
                    arr[row - 1, col - 1] = valueArray[row, col].ToString();
                }
            }
            // Close the Workbook
            xlWorkbook.Close(false);
            // Relase COM Object by decrementing the reference count
            Marshal.ReleaseComObject(xlWorkbook);
            // Close Excel application
            xlApp.Quit();
            // Release COM object
            Marshal.FinalReleaseComObject(xlApp);
            return arr;
        }

        private static string CreatePathUrl(string dateString)
        {
            return "https://www.gpw.pl/notowania_archiwalne?type=10&date=" + dateString + "&fetch.x=30&fetch.y=16";
        }
    }
}
