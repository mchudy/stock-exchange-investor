using System.Web.Mvc;

namespace StockExchange.Web
{
    public class FilterConfig
    {
        public static void RegisterFilters(GlobalFilterCollection filters)
        {
#if !DEBUG
            filters.Add(new RequireHttpsAttribute());
#endif
        }
    }
}