using StockExchange.Web.Filters;
using System.Web.Mvc;

namespace StockExchange.Web
{
    /// <summary>
    /// Global filter configuration
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Registers the global filters
        /// </summary>
        /// <param name="filters">Filters to register</param>
        public static void RegisterFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleJsonErrorAttribute());
#if !DEBUG
            filters.Add(new RequireHttpsAttribute());
#endif
        }
    }
}