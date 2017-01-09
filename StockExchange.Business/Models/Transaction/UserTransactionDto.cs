using StockExchange.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace StockExchange.Business.Models.Transaction
{
    /// <summary>
    /// Represents a real transaction concluded by the user on the stock exchange
    /// and entered into the system
    /// </summary>
    public class UserTransactionDto
    {
        /// <summary>
        /// The transaction ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The date of the transaction
        /// </summary>
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = Consts.Formats.DisplayDate)]
        public DateTime Date { get; set; }

        /// <summary>
        /// The price for a single stocks
        /// </summary>
        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        public decimal Price { get; set; }

        /// <summary>
        /// Number of stocks involved
        /// </summary>
        [Display(Name = "Quantity")]
        [DisplayFormat(DataFormatString = Consts.Formats.Integer)]
        public int Quantity { get; set; }

        /// <summary>
        /// Total value of involved stocks
        /// </summary>
        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        public decimal Total { get; set; }

        /// <summary>
        /// The company name
        /// </summary>
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        /// <summary>
        /// The achieved profit
        /// </summary>
        [Display(Name = "Profit")]
        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        public decimal Profit { get; set; }

        /// <summary>
        /// The action taken
        /// </summary>
        [Display(Name = "Action")]
        public Action Action { get; set; }

        /// <summary>
        /// The user ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The company ID
        /// </summary>
        public int CompanyId { get; set; }
    }

    /// <summary>
    /// The action taken during the transaction
    /// </summary>
    public enum Action
    {
        /// <summary>
        /// The stocks were bought by the user
        /// </summary>
        Buy = 1,

        /// <summary>
        /// The stocks were sold by the user
        /// </summary>
        Sell = 2
    }
}
