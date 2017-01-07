using System.ComponentModel.DataAnnotations.Schema;

namespace StockExchange.DataAccess.Models
{
    /// <summary>
    /// Property of an indicator used in a strategy
    /// </summary>
    [Table("StrategyIndicatorProperty")]
    public class StrategyIndicatorProperty
    {
        /// <summary>
        /// Property ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Property name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Property value
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Indicator
        /// </summary>
        public virtual StrategyIndicator Indicator { get; set; }

        /// <summary>
        /// Indicator ID
        /// </summary>
        public int IndicatorId { get; set; }
    }
}