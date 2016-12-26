using System;

namespace StockExchange.DataAccess.Models
{
    public class UserTransaction
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public virtual Company Company { get; set; }

        public int CompanyId { get; set; }

        public virtual User User { get; set; }

        public int UserId { get; set; }
    }
}
