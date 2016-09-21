using StockExchange.DataAccess.Models;

namespace StockExchange.DataAccess
{
    using System.Data.Entity;

    public sealed class StockExchangeModel : DbContext
    {
        public StockExchangeModel()
            : base("name=StockExchangeModel")
        {
        }

        public IDbSet<Company> Companies { get; set; }
        public IDbSet<Price> Prices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .Property(e => e.code)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Prices)
                .WithRequired(e => e.Company)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Price>()
                .Property(e => e.openPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Price>()
                .Property(e => e.closePrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Price>()
                .Property(e => e.highPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Price>()
                .Property(e => e.lowPrice)
                .HasPrecision(18, 0);
        }
    }
}
