using Autofac;
using Autofac.Integration.Mvc;
using StockExchange.Business.Indicators.Common;
using StockExchange.DataAccess;
using StockExchange.DataAccess.Cache;
using StockExchange.DataAccess.CachedRepositories;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
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

            RegisterRepositories(builder);

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
                builder.RegisterType<RedisCache>().AsImplementedInterfaces().SingleInstance();

                builder.RegisterType<PriceRepository>().Named<IPriceRepository>("base")
                    .Named<IRepository<Price>>("base");
                builder.RegisterDecorator<IPriceRepository>((c, inner) =>
                        new CachedPriceRepository(inner, c.Resolve<ICache>()), fromKey: "base");

                builder.RegisterType<TransactionsRepository>().Named<ITransactionsRepository>("base")
                    .Named<IRepository<UserTransaction>>("base");
                builder.RegisterDecorator<ITransactionsRepository>((c, inner) =>
                        new CachedTransactionRepository(inner, c.Resolve<ICache>()), fromKey: "base");
            }
            else
            {
                builder.RegisterType<NoCache>().AsImplementedInterfaces().SingleInstance();

                builder.RegisterType<PriceRepository>().AsImplementedInterfaces();
                builder.RegisterType<TransactionsRepository>().AsImplementedInterfaces();
            }

            builder.RegisterType<UserRepository>().AsImplementedInterfaces();
            builder.RegisterType<CompanyRepository>().AsImplementedInterfaces();
            builder.RegisterType<StrategiesRepository>().AsImplementedInterfaces();
        }
    }
}