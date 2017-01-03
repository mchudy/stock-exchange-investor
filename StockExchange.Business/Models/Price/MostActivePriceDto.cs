using System.ComponentModel.DataAnnotations;
using StockExchange.Common;

namespace StockExchange.Business.Models.Price
{
    public class MostActivePriceDto
    {
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        [Display(Name = "Price")]
        public decimal ClosePrice { get; set; }

        [DisplayFormat(DataFormatString = Consts.Formats.Integer)]
        [Display(Name = "Volume")]
        public int Volume { get; set; }

        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        [Display(Name = "Change [%]")]
        public decimal Change { get; set; }

        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        [Display(Name = "Turnover")]
        public decimal Turnover { get; set; }
    }
}
