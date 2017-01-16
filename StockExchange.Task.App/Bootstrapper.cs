using Autofac;
using StockExchange.Common;
using StockExchange.DataAccess.Cache;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using StockExchange.DataAccess.Repositories;
using StockExchange.Task.App.Commands;
using StockExchange.Task.App.Helpers;
using StockExchange.Task.Business;
using System.Configuration;
using System.Reflection;

namespace StockExchange.Task.App
{
    internal static class Bootstrapper
    {
        internal static IContainer Configure()
        {

            var assembly = Assembly.GetExecutingAssembly();
            var builder = new ContainerBuilder();
            builder.RegisterType<GenericRepository<Company>>().As<IRepository<Company>>();
            builder.RegisterType<Factory<GenericRepository<Price>>>().As<IFactory<IRepository<Price>>>();
            builder.RegisterType<GenericRepository<Price>>().As<IRepository<Price>>();
            builder.RegisterType<GenericRepository<CompanyGroup>>().AsImplementedInterfaces();
            builder.RegisterType<DataSynchronizer>().As<IDataSynchronizer>();
            builder.RegisterType<DataSynchronizerGpw>().As<IDataSynchronizerGpw>();
            builder.RegisterType<DataFixer>().AsImplementedInterfaces();
            builder.RegisterType<CompanyGroupsSynchronizer>().AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(assembly).Where(CommandHelper.IsCommand).Named<ICommand>(a => CommandHelper.GetCommandName(a).Name);

            bool useCache;
            bool.TryParse(ConfigurationManager.AppSettings["UseCache"], out useCache);
            if (useCache)
            {
                builder.RegisterType<RedisCache>().AsImplementedInterfaces();
            }
            else
            {
                builder.RegisterType<NoCache>().AsImplementedInterfaces();
            }

            return builder.Build();
        }
    }
}
