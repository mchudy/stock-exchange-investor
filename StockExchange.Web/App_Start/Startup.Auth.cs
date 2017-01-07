using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using StockExchange.DataAccess.Models;
using System;
// ReSharper disable ArgumentsStyleOther
// ReSharper disable ArgumentsStyleAnonymousFunction

namespace StockExchange.Web
{
    /// <summary>
    /// OWIN startup class of the application
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configures the authentication in the application
        /// </summary>
        /// <param name="app"></param>
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, User, int>(
                         validateInterval: TimeSpan.FromMinutes(30),
                         regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                         getUserIdCallback: id => id.GetUserId<int>()
                    )
                }
            });
        }
    }
}