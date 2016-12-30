using System.ComponentModel.DataAnnotations;

namespace StockExchange.Business.Models.Wallet
{
    public class OwnedCompanyStocksDto
    {
        public int UserId { get; set; }

        public int CompanyId { get; set; }

        [Display(Name = "Company")]
        public string CompanyName { get; set; }

        [Display(Name = "Quantity")]
        public int OwnedStocksCount { get; set; }

        /// <summary>
        /// Current price for a single stock
        /// </summary>
        [Display(Name = "Current Price")]
        public decimal CurrentPrice { get; set; }

        /// <summary>
        /// Represents total money put into stocks of the company
        /// </summary>
        [Display(Name = "Total Buy Price")]
        public decimal TotalBuyPrice { get; set; }

        [Display(Name = "Buy Price")]
        public decimal AverageBuyPrice { get; set; }

        /// <summary>
        /// Total value of all currently owned stocks of the company (according to 
        /// the current prices)
        /// </summary>
        [Display(Name = "Total Current Value")]
        public decimal CurrentValue { get; set; }

        public decimal Profit => CurrentValue - TotalBuyPrice;
    }
}
