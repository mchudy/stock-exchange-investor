namespace StockExchange.Business.Models
{
    public class OwnedCompanyStocksDto
    {
        public int UserId { get; set; }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        public int OwnedStocksCount { get; set; }

        /// <summary>
        /// Current price for a single stock
        /// </summary>
        public decimal CurrentPrice { get; set; }

        /// <summary>
        /// Represents total money put into stocks of the company
        /// </summary>
        public decimal TotalBuyPrice { get; set; }

        /// <summary>
        /// Total value of all currently owned stocks of the company (according to 
        /// the current prices)
        /// </summary>
        public decimal CurrentValue { get; set; }

        public decimal Profit => CurrentValue - TotalBuyPrice;
    }
}
