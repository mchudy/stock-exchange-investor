using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace StockExchange.Task.Business.Helpers
{
    internal static class CsvImporter
    {
        public static IList<IList<string>> GetCsv(string url)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            var resp = (HttpWebResponse)req.GetResponse();
            var ret = new List<IList<string>>();
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var sr = new StreamReader(resp.GetResponseStream()))
            {
                string currentLine;
                while ((currentLine = sr.ReadLine()) != null)
                {
                    ret.Add(currentLine.Split(',').ToList());
                }
                return ret;
            }
        }
    }
}
