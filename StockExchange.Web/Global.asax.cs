using log4net;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace StockExchange.Web
{
    /// <summary>
    /// Global application class
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MvcApplication));

        /// <summary>
        /// Invoked at the application start
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }

        /// <summary>
        /// Invoked when a cricital error occurred while processing the request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();
            log.Error("Application Error", ex);
        }

    }
}
