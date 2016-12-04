using System;

namespace StockExchange.Business.Models
{
    public class UserTransactionDto
    {
        public int CompanyId { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string CompanyName { get; set; }
    }
}
