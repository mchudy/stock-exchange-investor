using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using StockExchange.DataAccess.Models;
using StockExchange.Web.Helpers.ToastNotifications;
using StockExchange.Web.Models.Account;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StockExchange.Web.Controllers
{
    /// <summary>
    /// Controller for account actions
    /// </summary>
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly ApplicationSignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ApplicationUserManager _userManager;

        /// <summary>
        /// Creates a new instance of <see cref="AccountController"/>
        /// </summary>
        /// <param name="signInManager"></param>
        /// <param name="authenticationManager"></param>
        /// <param name="userManager"></param>
        public AccountController(ApplicationSignInManager signInManager, IAuthenticationManager authenticationManager, ApplicationUserManager userManager)
        {
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Returns a login view
        /// </summary>
        /// <param name="returnUrl">URL to redirect to</param>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// Loggs in the user
        /// </summary>
        /// <param name="model">View model</param>
        /// <param name="returnUrl">URL to redirect to</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                default:
                    ModelState.AddModelError("", "The provided username or password is incorrect.");
                    return View(model);
            }
        }

        /// <summary>
        /// Returns a register view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Registers the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return RedirectToAction("Index", "Wallet");
            }
            AddErrors(result);
            return View(model);
        }

        /// <summary>
        /// Logs out the users
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Charts");
        }

        /// <summary>
        /// Returns the settings view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Settings()
        {
            return View(new ChangePasswordViewModel());
        }

        /// <summary>
        /// Changes the user password
        /// </summary>
        /// <param name="model">View model</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Settings", model);

            var result = await _userManager.ChangePasswordAsync(CurrentUserId, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                AddErrors(result);
                return View("Settings", model);
            }
            ShowNotification("", "Password has been changed", ToastType.Success);
            return View("Settings");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Wallet");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}