using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Security.Principal;
// ReSharper disable SuggestBaseTypeForParameter

namespace StockExchange.DataAccess.Models
{
    /// <summary>
    /// A user login
    /// </summary>
    public class AppUserLogin : IdentityUserLogin<int> { }

    /// <summary>
    /// A user role
    /// </summary>
    public class AppUserRole : IdentityUserRole<int> { }

    /// <summary>
    /// A user claim
    /// </summary>
    public class AppUserClaim : IdentityUserClaim<int> { }

    /// <summary>
    /// A role in the application
    /// </summary>
    public class AppRole : IdentityRole<int, AppUserRole>
    {
        /// <summary>
        /// Creates a new instance of <see cref="AppRole"/>
        /// </summary>
        public AppRole()
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="AppRole"/>
        /// </summary>
        /// <param name="name"></param>
        public AppRole(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// An application user store
    /// </summary>
    public class ApplicationUserStore : UserStore<User, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>
    {
        /// <summary>
        /// Creates a new instance of <see cref="ApplicationUserStore"/>
        /// </summary>
        /// <param name="context"></param>
        public ApplicationUserStore(StockExchangeModel context) : base(context)
        {

        }
    }

    /// <summary>
    /// An application role store
    /// </summary>
    public class AppRoleStore : RoleStore<AppRole, int, AppUserRole>
    {
        /// <summary>
        /// Creates a new instance of <see cref="AppRoleStore"/>
        /// </summary>
        /// <param name="context"></param>
        public AppRoleStore(StockExchangeModel context) : base(context)
        {

        }
    }

    /// <summary>
    /// An application claims principal
    /// </summary>
    public class AppClaimsPrincipal : ClaimsPrincipal
    {
        /// <summary>
        /// Creates a new instance of <see cref="AppClaimsPrincipal"/>
        /// </summary>
        /// <param name="principal"></param>
        public AppClaimsPrincipal(IPrincipal principal) : base(principal)
        {

        }
    }
}
