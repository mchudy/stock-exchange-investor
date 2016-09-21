using System;
using System.Linq;
using log4net;
using Autofac;
using StockExchange.Task.App.Commands;

namespace StockExchange.Task.App
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            var logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            logger.Debug("Job started");
            logger.Debug("Parameters: " + string.Join(" ", args));
            var container = Bootstrapper.Configure();
            using (var scope = container.BeginLifetimeScope())
            {
                try
                {
                    var parameters = args.Skip(1).ToList();
                    if (args.Length > 0)
                    {
                        foreach (var arg in args)
                        {
                            var commandName = arg;
                            if (scope.IsRegisteredWithName(commandName, typeof (ICommand)))
                            {
                                // TODO: Task Log Db
                                scope.ResolveNamed<ICommand>(commandName).Execute(parameters);
                            }
                            else
                            {
                                scope.ResolveNamed<ICommand>("help").Execute(parameters);
                                logger.Error("Unknown command");
                            }
                        }
                    }
                    else
                    {
                        scope.ResolveNamed<ICommand>("help").Execute(parameters);
                        logger.Error("No command specified");
                    }
                }
                catch (Exception e)
                {
                    logger.ErrorFormat("Job error {0} Message: {1}{0}Inner Error: {2}{0}Stacktrace: " + e.StackTrace, Environment.NewLine, e.Message, e.InnerException?.Message ?? "");
                }
                finally
                {
                    logger.Debug("Job ended");
                    Console.ReadLine();
                }
            }
        }
    }
}
