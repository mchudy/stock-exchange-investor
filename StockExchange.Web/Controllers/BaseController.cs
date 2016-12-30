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
    public abstract class BaseController : Controller
    {
        public ApplicationUserManager UserManager { get; set; }

        protected int CurrentUserId => User.Identity.GetUserId<int>();

        private User _currentUser;

        protected User CurrentUser
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                    return null;
                return _currentUser ?? (_currentUser = UserManager.FindById(CurrentUserId));
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            InjectViewBagProperties();
        }

        protected ToastMessage ShowNotification(string title, string message, ToastType toastType = ToastType.Info)
        {
            NotificationsWrapper toastr = TempData["Notifications"] as NotificationsWrapper;
            toastr = toastr ?? new NotificationsWrapper();

            var toastMessage = toastr.AddToastMessage(title, message, toastType);
            TempData["Notifications"] = toastr;
            return toastMessage;
        }

        protected ActionResult JsonErrorResult(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            Response.StatusCode = (int) statusCode;
            return new JsonNetResult(new[] { new { message } });
        }

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