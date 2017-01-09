using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using StockExchange.DataAccess.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StockExchange.Web
{
    /// <summary>
    /// A user manager for the application (uses int as user ID)
    /// </summary>
    public class ApplicationUserManager : UserManager<User, int>
    {
        /// <summary>
        /// Creates a new instance of <see cref="ApplicationUserManager"/>
        /// </summary>
        /// <param name="store"></param>
        public ApplicationUserManager(IUserStore<User, int> store) : base(store)
        {
            UserValidator = new UserValidator<User, int>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6
            };
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;
        }
    }

    /// <summary>
    /// A sign in manager for the application (uses int as user ID)
    /// </summary>
    public class ApplicationSignInManager : SignInManager<User, int>
    {
        /// <summary>
        /// Creates a new <see cref="ApplicationSignInManager"/> instance
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="authenticationManager"></param>
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager)
        {

        }

        /// <summary>
        /// Creates a new user <see cref="ClaimsIdentity"/>
        /// </summary>
        /// <param name="user"></param>
        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        /// <summary>
        /// Creates a new <see cref="ApplicationSignInManager"/> instance
        /// </summary>
        /// <param name="options"></param>
        /// <param name="context"></param>
        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
