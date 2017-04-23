namespace StockExchange.Web.Models.Wallet
{
    /// <summary>
    /// Information about current user budget
    /// </summary>
    public class BudgetInfoViewModel
    {
        /// <summary>
        /// Free budget
        /// </summary>
        public decimal FreeBudget { get; set; }

        /// <summary>
        /// Value of all owned stocks
        /// </summary>
        public decimal AllStocksValue { get; set; }

        /// <summary>
        /// Total budget
        /// </summary>
        public decimal TotalBudget => FreeBudget + AllStocksValue;
    }
}