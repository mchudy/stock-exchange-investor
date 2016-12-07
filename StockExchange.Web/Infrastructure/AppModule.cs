using Autofac;
using Autofac.Integration.Mvc;
using StockExchange.Business.Indicators;
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
            builder.RegisterType<GenericRepository<User>>().As<IRepository<User>>();
            builder.RegisterType<GenericRepository<InvestmentStrategy>>().As<IRepository<InvestmentStrategy>>();

            builder.RegisterAssemblyTypes(typeof(IIndicator).Assembly)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Services"))
                .AsImplementedInterfaces();

            builder.RegisterType<StockExchangeModel>().InstancePerRequest();
            builder.RegisterType<IndicatorFactory>().AsImplementedInterfaces().SingleInstance();
        }
    }
}