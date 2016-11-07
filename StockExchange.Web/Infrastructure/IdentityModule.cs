using Autofac;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using StockExchange.DataAccess.Models;
using System.Web;

namespace StockExchange.Web.Infrastructure
{
    public class IdentityModule : Module
    {
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

            builder.Register<IdentityFactoryOptions<ApplicationUserManager>>(c => new IdentityFactoryOptions<ApplicationUserManager>()
            {
                DataProtectionProvider = new DpapiDataProtectionProvider("StockExchange")
            });

            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication)
                .InstancePerRequest();

            builder.Register(c => c.Resolve<ApplicationUserManager>()
                .FindById(HttpContext.Current.User.Identity.GetUserId<int>())).As<User>();
        }
    }
}