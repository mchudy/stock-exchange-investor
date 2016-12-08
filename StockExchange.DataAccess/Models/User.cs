using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Models
{
    // ReSharper disable once RedundantExtendsListEntry
    public class User : IdentityUser<int, AppUserLogin, AppUserRole, AppUserClaim>, IUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public decimal Budget { get; set; }

        public ICollection<UserTransaction> Transactions { get; set; } = new HashSet<UserTransaction>();

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager)
        {
            return await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}
