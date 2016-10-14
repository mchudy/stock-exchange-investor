using Autofac;
using Autofac.Integration.Mvc;
using log4net;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace StockExchange.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MvcApplication));

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(MvcApplication).Assembly);
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();
            log.Error("App_Error", ex);
        }

    }
}
