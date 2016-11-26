using Microsoft.AspNet.Identity.EntityFramework;
using StockExchange.DataAccess.Models;

namespace StockExchange.DataAccess
{
    using System.Data.Entity;

    public sealed class StockExchangeModel : IdentityDbContext<User, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>
    {
        public StockExchangeModel() : base("name=StockExchangeModel")
        {

        }

        public IDbSet<Company> Companies { get; set; }

        public IDbSet<Price> Prices { get; set; }

        public IDbSet<InvestmentStrategy> Strategies { get; set; }

        public IDbSet<InvestmentCondition> Conditions { get; set; }
    }
}
