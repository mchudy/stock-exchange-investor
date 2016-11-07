using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace StockExchange.Web.Helpers
{
    public static class HtmlExtension
    {
        public static string IsSelected(this HtmlHelper html, string controllers = "", string actions = "", string cssClass = "active")
        {
            ViewContext viewContext = html.ViewContext;
            bool isChildAction = viewContext.Controller.ControllerContext.IsChildAction;

            if (isChildAction)
                viewContext = html.ViewContext.ParentActionViewContext;

            RouteValueDictionary routeValues = viewContext.RouteData.Values;
            string currentAction = routeValues["action"].ToString();
            string currentController = routeValues["controller"].ToString();

            if (string.IsNullOrEmpty(actions))
                actions = currentAction;

            if (string.IsNullOrEmpty(controllers))
                controllers = currentController;

            string[] acceptedActions = actions.Trim().Split(',').Distinct().ToArray();
            string[] acceptedControllers = controllers.Trim().Split(',').Distinct().ToArray();

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
                cssClass : string.Empty;
        }

        public static MvcHtmlString DropDownList(this HtmlHelper htmlHelper, string name, IEnumerable<string> options, object htmlAttributes = null)
        {
            var items = options.Select(o => new SelectListItem { Value = o, Text = o, Selected = o == null });
            items = SingleEmptyItem.Concat(items);
            return htmlHelper.DropDownList(name, items, htmlAttributes);
        }

        private static readonly SelectListItem[] SingleEmptyItem = { new SelectListItem { Text = "", Value = "" } };
    }
}