using System;
using System.ComponentModel.DataAnnotations;
using StockExchange.Common;

namespace StockExchange.Business.Models.Price
{
    public sealed class PriceDto
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Id")]
        public int CompanyId { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        [Display(Name = "Open")]
        public decimal OpenPrice { get; set; }

        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        [Display(Name = "Close")]
        public decimal ClosePrice { get; set; }

        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        [Display(Name = "High")]
        public decimal HighPrice { get; set; }

        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        [Display(Name = "Low")]
        public decimal LowPrice { get; set; }

        [DisplayFormat(DataFormatString = Consts.Formats.Integer)]
        [Display(Name = "Volume")]
        public int Volume { get; set; }
    }
}
