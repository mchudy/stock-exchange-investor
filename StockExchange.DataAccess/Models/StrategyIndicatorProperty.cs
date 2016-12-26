using System.ComponentModel.DataAnnotations.Schema;

namespace StockExchange.DataAccess.Models
{
    [Table("StrategyIndicatorProperty")]
    public class StrategyIndicatorProperty
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Value { get; set; }

        public virtual StrategyIndicator Indicator { get; set; }

        public int IndicatorId { get; set; }
    }
}