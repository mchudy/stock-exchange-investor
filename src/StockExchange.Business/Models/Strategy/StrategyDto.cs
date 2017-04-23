using StockExchange.Business.Models.Indicators;
using System.Collections.Generic;

namespace StockExchange.Business.Models.Strategy
{
    /// <summary>
    /// Represents a trading strategy
    /// </summary>
    public class StrategyDto
    {
        /// <summary>
        /// Id of the strategy
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the strategy
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The id of user who created the strategy
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Number of days within all specified signals must occur to take action.
        /// If null, any signal might generate a buy or sell transaction.
        /// </summary>
        public int? SignalDaysPeriod { get; set; }

        /// <summary>
        /// Indicates whether the action might be taken when any signal
        /// from any indicator is generated
        /// </summary>
        public bool IsDisjunctiveStrategy => SignalDaysPeriod == null;

        /// <summary>
        /// Indicates whether the action might be taken only when all indicators
        /// generate signals in a specified period of time
        /// </summary>
        public bool IsConjunctiveStrategy => SignalDaysPeriod != null;

        /// <summary>
        /// Technical indicators used in the strategy
        /// </summary>
        public IList<ParameterizedIndicator> Indicators { get; set; }
    }
}
