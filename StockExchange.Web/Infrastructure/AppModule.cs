using Autofac;
using Autofac.Integration.Mvc;
using StockExchange.Business.Business;
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
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<GenericRepository<Company>>().As<IRepository<Company>>();
            builder.RegisterType<GenericRepository<Price>>().As<IRepository<Price>>();
            builder.RegisterType<PriceManager>().As<IPriceManager>();
            builder.RegisterType<StockExchangeModel>().InstancePerRequest();
        }
    }
}