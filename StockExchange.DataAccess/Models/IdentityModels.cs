using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Security.Principal;

namespace StockExchange.DataAccess.Models
{
    public class AppUserLogin : IdentityUserLogin<int> { }

    public class AppUserRole : IdentityUserRole<int> { }

    public class AppUserClaim : IdentityUserClaim<int> { }

    public class AppRole : IdentityRole<int, AppUserRole>
    {
        public AppRole()
        { }

        public AppRole(string name)
        {
            Name = name;
        }
    }

    public class ApplicationUserStore :
        UserStore<User, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>
    {

        public ApplicationUserStore(StockExchangeModel context) : base(context)
        {

        }
    }

    public class AppRoleStore : RoleStore<AppRole, int, AppUserRole>
    {
        public AppRoleStore(StockExchangeModel context)
            : base(context)
        {
        }
    }

    public class AppClaimsPrincipal : ClaimsPrincipal
    {
        public AppClaimsPrincipal(IPrincipal principal) : base(principal)
        { }

    }
}
