using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace StockExchange.Web.Helpers
{
    public static class HtmlExtension
    {
        public static MvcHtmlString DropDownList(this HtmlHelper htmlHelper, string name, IEnumerable<string> options, object htmlAttributes = null)
        {
            var items = options.Select(o => new SelectListItem { Value = o, Text = o, Selected = o == null });
                items = SingleEmptyItem.Concat(items);
            return htmlHelper.DropDownList(name, items, htmlAttributes);
        }

        private static readonly SelectListItem[] SingleEmptyItem = { new SelectListItem { Text = "", Value = "" } };
    }
}