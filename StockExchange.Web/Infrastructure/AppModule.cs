using Autofac;
using Autofac.Integration.Mvc;
using StockExchange.Business.Indicators.Common;
using StockExchange.DataAccess;
using StockExchange.DataAccess.Cache;
using StockExchange.DataAccess.CachedRepositories;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Repositories;
using System.Web.Configuration;
// ReSharper disable ArgumentsStyleStringLiteral

namespace StockExchange.Web.Infrastructure
{
    /// <summary>
    /// Autofac module for the application
    /// </summary>
    public class AppModule : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();

            builder.RegisterType<RedisCache>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<TraceSection>().AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(IIndicator).Assembly)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Services"))
                .AsImplementedInterfaces();
            builder.RegisterType<StockExchangeModel>().InstancePerRequest();
            builder.RegisterType<IndicatorFactory>().AsImplementedInterfaces().SingleInstance();
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            bool useCache;
            bool.TryParse(WebConfigurationManager.AppSettings["UseCache"], out useCache);

            if (useCache)
            {
                builder.RegisterType<PriceRepository>().Named<IPriceRepository>("base");
                builder.RegisterDecorator<IPriceRepository>((c, inner) =>
                        new CachedPriceRepository(inner, c.Resolve<ICache>()), fromKey: "base");
            }
            else
            {
                builder.RegisterType<PriceRepository>().AsImplementedInterfaces();
            }

            builder.RegisterType<UserRepository>().AsImplementedInterfaces();
            builder.RegisterType<CompanyRepository>().AsImplementedInterfaces();
            builder.RegisterType<StrategiesRepository>().AsImplementedInterfaces();
            builder.RegisterType<TransactionsRepository>().AsImplementedInterfaces();
        }
    }
}