using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.Owin;
using Owin;
using StockExchange.Web;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(Startup))]
namespace StockExchange.Web
{
    /// <summary>
    /// OWIN startup class of the application
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configures the application
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(Startup).Assembly);
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            app.UseAutofacMiddleware(container);
            ConfigureAuth(app);
        }
    }
}