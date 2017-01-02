namespace StockExchange.Web.Models.Wallet
{
    public class BudgetInfoViewModel
    {
        public decimal FreeBudget { get; set; }

        public decimal AllStocksValue { get; set; }

        public decimal TotalBudget => FreeBudget + AllStocksValue;
    }
}