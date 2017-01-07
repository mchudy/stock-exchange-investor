using StockExchange.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace StockExchange.Business.Models.Price
{
    /// <summary>
    /// Represents a price of stocks on a single day
    /// </summary>
    public sealed class PriceDto
    {
        /// <summary>
        /// The price ID
        /// </summary>
        [Display(Name = "ID")]
        public int Id { get; set; }

        /// <summary>
        /// The company name
        /// </summary>
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        /// <summary>
        /// The company ID
        /// </summary>
        [Display(Name = "Company Id")]
        public int CompanyId { get; set; }

        /// <summary>
        /// The date
        /// </summary>
        [DisplayFormat(DataFormatString = Consts.Formats.DisplayDate)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// The open price
        /// </summary>
        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        [Display(Name = "Open")]
        public decimal OpenPrice { get; set; }

        /// <summary>
        /// The close price
        /// </summary>
        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        [Display(Name = "Close")]
        public decimal ClosePrice { get; set; }

        /// <summary>
        /// The maximum price of stocks during the day
        /// </summary>
        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        [Display(Name = "High")]
        public decimal HighPrice { get; set; }

        /// <summary>
        /// The minimum price of stocks during the day
        /// </summary>
        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        [Display(Name = "Low")]
        public decimal LowPrice { get; set; }

        /// <summary>
        /// The number of transactions during the day
        /// </summary>
        [DisplayFormat(DataFormatString = Consts.Formats.Integer)]
        [Display(Name = "Volume")]
        public int Volume { get; set; }
    }
}
