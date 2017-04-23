using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockExchange.DataAccess.Models
{
    /// <summary>
    /// Represents a trading strategy
    /// </summary>
    [Table("Strategy")]
    public class InvestmentStrategy
    {
        /// <summary>
        /// The strategy ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The user ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The user who created the strategy
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// The strategy name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Number of days within all specified signals must occur to take action.
        /// If null, any signal might generate a buy or sell transaction.
        /// </summary>
        public int? SignalDaysPeriod { get; set; }

        /// <summary>
        /// Whether the strategy has been deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Indicators used in the strategy
        /// </summary>
        public virtual ICollection<StrategyIndicator> Indicators { get; set; } = new HashSet<StrategyIndicator>();
    }
}
