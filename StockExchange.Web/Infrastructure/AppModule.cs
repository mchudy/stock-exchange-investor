using Autofac;
using Autofac.Integration.Mvc;
using StockExchange.Business.Services;
using StockExchange.DataAccess;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using StockExchange.DataAccess.Repositories;

namespace StockExchange.Web.Infrastructure
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();

            builder.RegisterType<GenericRepository<Company>>().As<IRepository<Company>>();
            builder.RegisterType<GenericRepository<Price>>().As<IRepository<Price>>();
            builder.RegisterType<GenericRepository<Wallet>>().As<IRepository<Wallet>>();
            builder.RegisterType<GenericRepository<User>>().As<IRepository<User>>();

            builder.RegisterType<PriceService>().As<IPriceService>();
            builder.RegisterType<WalletSerivce>().As<IWalletService>();

            builder.RegisterType<StockExchangeModel>().InstancePerRequest();
        }
    }
}