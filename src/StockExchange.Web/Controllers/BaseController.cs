using Microsoft.AspNet.Identity;
using StockExchange.Business.ErrorHandling;
using StockExchange.DataAccess.Models;
using StockExchange.Web.Helpers.Json;
using StockExchange.Web.Helpers.ToastNotifications;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace StockExchange.Web.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// User manager
        /// </summary>
        public ApplicationUserManager UserManager { get; set; }

        /// <summary>
        /// ID of a currently logged in user
        /// </summary>
        protected int CurrentUserId => User.Identity.GetUserId<int>();

        private User _currentUser;

        /// <summary>
        /// Currently logged in user
        /// </summary>
        protected User CurrentUser
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                    return null;
                return _currentUser ?? (_currentUser = UserManager.FindById(CurrentUserId));
            }
        }

        /// <inheritdoc />
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            InjectViewBagProperties();
        }

        /// <summary>
        /// Saves a toast notification to show
        /// </summary>
        /// <param name="title">Notification title</param>
        /// <param name="message">Notification message</param>
        /// <param name="toastType">Notification type</param>
        /// <returns>Notification</returns>
        protected ToastMessage ShowNotification(string title, string message, ToastType toastType = ToastType.Info)
        {
            NotificationsWrapper toastr = TempData["Notifications"] as NotificationsWrapper;
            toastr = toastr ?? new NotificationsWrapper();

            var toastMessage = toastr.AddToastMessage(title, message, toastType);
            TempData["Notifications"] = toastr;
            return toastMessage;
        }

        /// <summary>
        /// Returns an error result for the AJAX request
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="statusCode">HTTP status code to return</param>
        protected ActionResult JsonErrorResult(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            Response.StatusCode = (int) statusCode;
            return new JsonNetResult(new[] { new { message } });
        }

        /// <summary>
        /// Returns an error result for the AJAX request
        /// </summary>
        /// <param name="message">Error messages dictionary</param>
        /// <param name="statusCode">HTTP status code to return</param>
        protected ActionResult JsonErrorResult(ModelStateDictionary message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            Response.StatusCode = (int)statusCode;
            return new JsonNetResult(message.Values.SelectMany(e => e.Errors)
                .Select(e => new ValidationError("", e.ErrorMessage)));
        }

        private void InjectViewBagProperties()
        {
            ViewBag.CurrentUserFullName = CurrentUser?.FullName;
        }
    }
}