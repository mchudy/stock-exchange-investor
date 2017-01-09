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
#if !DEBUG
            filters.Add(new RequireHttpsAttribute());
#endif
        }
    }
}