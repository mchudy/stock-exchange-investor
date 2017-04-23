using System.Collections.Generic;

namespace StockExchange.DataAccess.Models
{
    /// <summary>
    /// An technical indicator used in a strategy
    /// </summary>
    public class StrategyIndicator
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The type of the indicator
        /// </summary>
        public int IndicatorType { get; set; }

        /// <summary>
        /// Strategy
        /// </summary>
        public virtual InvestmentStrategy Strategy { get; set; }

        /// <summary>
        /// Strategy ID
        /// </summary>
        public int StrategyId { get; set; }

        /// <summary>
        /// Indicator properties
        /// </summary>
        public virtual ICollection<StrategyIndicatorProperty> Properties { get; set; } = new HashSet<StrategyIndicatorProperty>(); 
    }
}
