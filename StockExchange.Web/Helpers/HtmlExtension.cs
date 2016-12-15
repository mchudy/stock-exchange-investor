using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace StockExchange.Web.Helpers
{
    public static class HtmlExtension
    {
        public static string IsSelected(this HtmlHelper html, string controllers = "", string actions = "", string cssClass = "active")
        {
            var viewContext = html.ViewContext;
            var isChildAction = viewContext.Controller.ControllerContext.IsChildAction;

            if (isChildAction)
                viewContext = html.ViewContext.ParentActionViewContext;

            var routeValues = viewContext.RouteData.Values;
            var currentAction = routeValues["action"].ToString();
            var currentController = routeValues["controller"].ToString();

            if (string.IsNullOrEmpty(actions))
                actions = currentAction;

            if (string.IsNullOrEmpty(controllers))
                controllers = currentController;

            var acceptedActions = actions.Trim().Split(',').Distinct().ToArray();
            var acceptedControllers = controllers.Trim().Split(',').Distinct().ToArray();

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
                cssClass : string.Empty;
        }

        public static MvcHtmlString DropDownList(this HtmlHelper htmlHelper, string name, IEnumerable<string> options, object htmlAttributes = null)
        {
            var items = options.Select(o => new SelectListItem { Value = o, Text = o, Selected = o == null });
            items = SingleEmptyItem.Concat(items);
            return htmlHelper.DropDownList(name, items, htmlAttributes);
        }

        public static IHtmlString ToJsonString(this HtmlHelper htmlHelper, object model)
        {
            return htmlHelper.Raw(JsonConvert.SerializeObject(model,
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
        }

        private static readonly SelectListItem[] SingleEmptyItem = { new SelectListItem { Text = "", Value = "" } };
    }
}