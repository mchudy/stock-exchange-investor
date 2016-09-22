using System.Reflection;
using Autofac;
using StockExchange.Common;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using StockExchange.DataAccess.Repositories;
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
            builder.RegisterType<Factory<GenericRepository<Company>>>().As<IFactory<IRepository<Company>>>();
            builder.RegisterType<Factory<GenericRepository<Price>>>().As<IFactory<IRepository<Price>>>();
            builder.RegisterType<DataSynchronizer>().As<IDataSynchronizer>();
            builder.RegisterAssemblyTypes(assembly).Where(CommandHelper.IsCommand).Named<ICommand>(a => CommandHelper.GetCommandName(a).Name);
            return builder.Build();
        }
    }
}
