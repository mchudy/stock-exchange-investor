using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace StockExchange.Task.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GlobalContext.Properties["LogName"] = args.Length > 0 ? args[0] : "";
            var logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            logger.Debug("Job started");
            logger.Debug("Parameters: " + string.Join(" ", args));
            Console.ReadLine();
        }
    }
}
