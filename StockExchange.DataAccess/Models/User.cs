using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Models
{
    // ReSharper disable once RedundantExtendsListEntry
    /// <summary>
    /// Represents an application user
    /// </summary>
    public class User : IdentityUser<int, AppUserLogin, AppUserRole, AppUserClaim>, IUser<int>
    {
        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Full name
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// Current free budget
        /// </summary>
        public decimal Budget { get; set; }

        /// <summary>
        /// User transactions
        /// </summary>
        public ICollection<UserTransaction> Transactions { get; set; } = new HashSet<UserTransaction>();

        /// <summary>
        /// Strategies defined by the user
        /// </summary>
        public ICollection<InvestmentStrategy> Strategies { get; set; } = new HashSet<InvestmentStrategy>();

        /// <summary>
        /// Creates a new user's <see cref="ClaimsIdentity"/>
        /// </summary>
        /// <param name="manager"></param>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager)
        {
            return await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}
