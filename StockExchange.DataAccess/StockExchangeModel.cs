using Microsoft.AspNet.Identity.EntityFramework;
using StockExchange.DataAccess.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace StockExchange.DataAccess
{
    using System.Data.Entity;

    /// <summary>
    /// A StockExchange database model
    /// </summary>
    public class StockExchangeModel : IdentityDbContext<User, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>
    {
        /// <summary>
        /// Creates a new instance of <see cref="StockExchangeModel"/>
        /// </summary>
        public StockExchangeModel() : base("name=StockExchangeModel")
        {
#if DEBUG
            //Database.Log = s => { Debug.Write(s); };
#endif
        }

        /// <summary>
        /// The companies table
        /// </summary>
        public IDbSet<Company> Companies { get; set; }

        /// <summary>
        /// The prices table
        /// </summary>
        public virtual IDbSet<Price> Prices { get; set; }

        /// <summary>
        /// The user transactions table
        /// </summary>
        public IDbSet<UserTransaction> UserTransactions { get; set; }

        /// <summary>
        /// The strategies table
        /// </summary>
        public IDbSet<InvestmentStrategy> Strategies { get; set; }

        /// <summary>
        /// The strategy indicators table
        /// </summary>
        public IDbSet<StrategyIndicator> StrategyIndicators { get; set; }

        /// <summary>
        /// The table for indicator properties
        /// </summary>
        public IDbSet<StrategyIndicatorProperty> StrategyIndicatorProperties { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
