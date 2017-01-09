using StockExchange.Common;
using System.ComponentModel.DataAnnotations;

namespace StockExchange.Business.Models.Price
{
    /// <summary>
    /// Contains information about a most active companies compared to the
    /// previous day
    /// </summary>
    public class MostActivePriceDto
    {
        /// <summary>
        /// The company name
        /// </summary>
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        /// <summary>
        /// The close price
        /// </summary>
        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        [Display(Name = "Price")]
        public decimal ClosePrice { get; set; }

        /// <summary>
        /// The total number of transaction concluded during the day
        /// </summary>
        [DisplayFormat(DataFormatString = Consts.Formats.Integer)]
        [Display(Name = "Volume")]
        public int Volume { get; set; }

        /// <summary>
        /// The change in volume 
        /// </summary>
        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        [Display(Name = "Change [%]")]
        public decimal Change { get; set; }
    }
}
