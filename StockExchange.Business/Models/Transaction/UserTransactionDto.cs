using System;
using System.ComponentModel.DataAnnotations;

namespace StockExchange.Business.Models.Transaction
{
    public class UserTransactionDto
    {
        public int Id { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        public int UserId { get; set; }

        public DataAccess.Models.Company Company { get; set; }

        [Display(Name = "CompanyId")]
        public int CompanyId { get; set; }
    }
}
