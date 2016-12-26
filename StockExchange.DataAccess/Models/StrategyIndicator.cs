using System.Collections.Generic;

namespace StockExchange.DataAccess.Models
{
    public class StrategyIndicator
    {
        public int Id { get; set; }

        public int IndicatorType { get; set; }

        public virtual InvestmentStrategy Strategy { get; set; }

        public int StrategyId { get; set; }

        public virtual ICollection<StrategyIndicatorProperty> Properties { get; set; } = new HashSet<StrategyIndicatorProperty>(); 
    }
}
