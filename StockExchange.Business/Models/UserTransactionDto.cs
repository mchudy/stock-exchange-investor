using StockExchange.DataAccess.Models;
using System;

namespace StockExchange.Business.Models
{
    public class UserTransactionDto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int UserId { get; set; }

        public Company Company { get; set; }

        public int CompanyId { get; set; }
    }
}
