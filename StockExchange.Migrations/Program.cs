using DbUp;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace StockExchange.Migrations
{
    internal sealed class Program
    {
        private const string ProductionOptionName = "prod";

        private static int Main(string[] args)
        {
            var connectionString = GetConnectionString(args);
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No connection string provided!");
                Console.ResetColor();
                return -1;
            }

            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            Console.WriteLine($"Running migration on {connectionStringBuilder.DataSource}");
            var upgrader = DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .WithTransaction()
                    .LogToConsole()
                    .Build();
            var result = upgrader.PerformUpgrade();
            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }

        private static string GetConnectionString(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings?["LocalConnection"]?.ConnectionString;
            string arg = args.FirstOrDefault(a => a.Trim() != "--fromconsole");
            if (arg == ProductionOptionName)
            {
                connectionString = ConfigurationManager.ConnectionStrings?["ProductionConnection"]?.ConnectionString;
            }
            else if (!string.IsNullOrWhiteSpace(arg))
            {
                connectionString = arg;
            }
            return connectionString;
        }
    }
}
