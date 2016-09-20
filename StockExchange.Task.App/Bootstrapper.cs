using System.Reflection;
using Autofac;
using StockExchange.Task.App.Commands;
using StockExchange.Task.App.Helpers;
using StockExchange.Task.Business;

namespace StockExchange.Task.App
{
    internal static class Bootstrapper
    {
        internal static IContainer Configure()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var builder = new ContainerBuilder();
            builder.RegisterType<HistoricalDataSynchronizer>().As<IHistoricalDataSynchronizer>();
            builder.RegisterAssemblyTypes(assembly).Where(CommandHelper.IsCommand).Named<ICommand>(a => CommandHelper.GetCommandName(a).Name);
            return builder.Build();
        }
    }
}
