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
        /// Technical indicators used in the strategy
        /// </summary>
        public IList<ParameterizedIndicator> Indicators { get; set; }
    }
}
