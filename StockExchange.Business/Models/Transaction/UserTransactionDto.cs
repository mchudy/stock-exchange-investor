using System;

namespace StockExchange.Business.Models.Transaction
{
    public class UserTransactionDto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int UserId { get; set; }

        public DataAccess.Models.Company Company { get; set; }

        public int CompanyId { get; set; }
    }
}
