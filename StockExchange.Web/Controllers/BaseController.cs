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
                if (_currentUser != null || !User.Identity.IsAuthenticated) return null;
                _currentUser = UserManager.FindById(CurrentUserId);
                return _currentUser;
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