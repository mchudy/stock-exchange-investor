using Autofac;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using StockExchange.DataAccess.Models;
using System.Web;

namespace StockExchange.Web.Infrastructure
{
    /// <summary>
    /// Autofac module for ASP.NET Identity services
    /// </summary>
    public class IdentityModule : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationUserStore>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterType<ApplicationUserManager>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<ApplicationSignInManager>()
                .AsSelf()
                .InstancePerRequest();

            builder.Register(c => new IdentityFactoryOptions<ApplicationUserManager>()
            {
                DataProtectionProvider = new DpapiDataProtectionProvider("StockExchange")
            });

            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication)
                .InstancePerRequest();
        }
    }
}