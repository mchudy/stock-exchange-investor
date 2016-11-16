using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.Owin;
using Owin;
using StockExchange.Web;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(Startup))]
namespace StockExchange.Web
{
    public partial class Startup
    {
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