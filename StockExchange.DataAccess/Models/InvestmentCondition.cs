using System.ComponentModel.DataAnnotations.Schema;

namespace StockExchange.DataAccess.Models
{
    [Table("Condition")]
    public class InvestmentCondition
    {
        public int Id { get; set; }
        public string IndicatorName { get; set; }

        public decimal? BuyValue { get; set; }
        public decimal? SellValue { get; set; }

        public virtual InvestmentStrategy Strategy { get; set; }
    }
}
