using Microsoft.AspNet.Identity;
using StockExchange.DataAccess.Models;
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

        private void InjectViewBagProperties()
        {
            ViewBag.CurrentUserFullName = CurrentUser?.FullName;
        }
    }
}