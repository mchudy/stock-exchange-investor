using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace StockExchange.Web.Helpers
{
    /// <summary>
    /// Extension methods for <see cref="HtmlHelper"/>
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Provides an easy way to mark HTML elements as selected
        /// </summary>
        /// <param name="html"></param>
        /// <param name="controllers">Controllers which should trigger the selection, separated by commas</param>
        /// <param name="actions">Controller actions which should trigger the selection, separated by commas</param>
        /// <param name="cssClass">CSS class to add when element is selected</param>
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

        /// <summary>
        /// Renders a drop down list
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name">Name of the select element</param>
        /// <param name="options">Options</param>
        /// <param name="htmlAttributes">HTML attributes for the select</param>
        public static MvcHtmlString DropDownList(this HtmlHelper htmlHelper, string name, IEnumerable<string> options, object htmlAttributes = null)
        {
            var items = options.Select(o => new SelectListItem { Value = o, Text = o, Selected = o == null });
            items = SingleEmptyItem.Concat(items);
            return htmlHelper.DropDownList(name, items, htmlAttributes);
        }

        /// <summary>
        /// Renders a JSON string from the given object
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="model">Object to serialize</param>
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