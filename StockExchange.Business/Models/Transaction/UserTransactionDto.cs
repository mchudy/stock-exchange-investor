using System;
using System.ComponentModel.DataAnnotations;
using StockExchange.Common;

namespace StockExchange.Business.Models.Transaction
{
    public class UserTransactionDto
    {
        public int Id { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = Consts.Formats.DisplayDate)]
        public DateTime Date { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Quantity")]
        [DisplayFormat(DataFormatString = Consts.Formats.Integer)]
        public int Quantity { get; set; }

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        public decimal Total { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Profit")]
        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        public decimal Profit { get; set; }

        [Display(Name = "Action")]
        public Action Action { get; set; }

        public int UserId { get; set; }

        public int CompanyId { get; set; }
    }

    public enum Action
    {
        Buy = 1,
        Sell = 2
    }
}
