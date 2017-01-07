using StockExchange.Business.Models.Company;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockExchange.Web.Models.Wallet
{
    /// <summary>
    /// View model for adding a new user transaction
    /// </summary>
    public class AddTransactionViewModel
    {
        /// <summary>
        /// All companies available
        /// </summary>
        public IList<CompanyDto> Companies { get; set; } = new List<CompanyDto>();

        /// <summary>
        /// Chosen company ID
        /// </summary>
        [Required]
        public int SelectedCompanyId { get; set; }

        /// <summary>
        /// Price of a single stock
        /// </summary>
        [DataType(DataType.Currency)]
        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// Number of stocks involved
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 1")]
        public int Quantity { get; set; }

        /// <summary>
        /// Transaction type
        /// </summary>
        public TransactionActionType TransactionType { get; set; } = TransactionActionType.Buy;

        /// <summary>
        /// Date
        /// </summary>
        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Transaction type
    /// </summary>
    public enum TransactionActionType
    {
        /// <summary>
        /// Stocks have been bought
        /// </summary>
        Buy,

        /// <summary>
        /// Stocks have been sold
        /// </summary>
        Sell
    }
}