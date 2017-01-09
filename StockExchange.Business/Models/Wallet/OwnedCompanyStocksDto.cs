using StockExchange.Common;
using System.ComponentModel.DataAnnotations;

namespace StockExchange.Business.Models.Wallet
{
    /// <summary>
    /// Contains information about the company's stocks owned by the user
    /// </summary>
    public class OwnedCompanyStocksDto
    {
        /// <summary>
        /// User ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Company ID
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Company Name
        /// </summary>
        [Display(Name = "Company")]
        public string CompanyName { get; set; }

        /// <summary>
        /// The number of currently owned stocks
        /// </summary>
        [Display(Name = "Quantity")]
        [DisplayFormat(DataFormatString = Consts.Formats.Integer)]
        public int OwnedStocksCount { get; set; }

        /// <summary>
        /// Current price for a single stock
        /// </summary>
        [Display(Name = "Current Price")]
        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        public decimal CurrentPrice { get; set; }

        /// <summary>
        /// Represents total money put into stocks of the company
        /// </summary>
        [Display(Name = "Total Buy Price")]
        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        public decimal TotalBuyPrice { get; set; }

        /// <summary>
        /// Average price for which user bought the stocks
        /// </summary>
        [Display(Name = "Buy Price")]
        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        public decimal AverageBuyPrice { get; set; }

        /// <summary>
        /// Total value of all currently owned stocks of the company (according to 
        /// the current prices)
        /// </summary>
        [Display(Name = "Total Current Value")]
        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        public decimal CurrentValue { get; set; }

        /// <summary>
        /// Overall profit achieved on the company's stocks
        /// </summary>
        [Display(Name = "Profit")]
        [DisplayFormat(DataFormatString = Consts.Formats.Currency)]
        public decimal Profit => CurrentValue - TotalBuyPrice;
    }
}
