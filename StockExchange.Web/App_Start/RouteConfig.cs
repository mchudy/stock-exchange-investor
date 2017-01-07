using System.Web.Mvc;
using System.Web.Routing;

namespace StockExchange.Web
{
    /// <summary>
    /// Global routing configuration
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Registers the routes
        /// </summary>
        /// <param name="routes">Routes to register</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Charts", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
