using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StockExchange.Business.Models.Company;

namespace StockExchange.Web.Models.Wallet
{
    public class AddTransactionViewModel
    {
        public IList<CompanyDto> Companies { get; set; } = new List<CompanyDto>();

        [Required]
        public int SelectedCompanyId { get; set; }

        [DataType(DataType.Currency)]
        [Required]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 1")]
        public int Quantity { get; set; }

        public TransactionActionType TransactionType { get; set; } = TransactionActionType.Buy;

        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
    }

    public enum TransactionActionType
    {
        Buy,
        Sell
    }
}