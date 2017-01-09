using StockExchange.Web.Models.Indicator;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockExchange.Web.Models.Strategy
{
    /// <summary>
    /// View model for editing strategy
    /// </summary>
    public class EditStrategyViewModel
    {
        /// <summary>
        /// Strategy Id
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Strategy Name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Chosen indicators
        /// </summary>
        public IList<EditIndicatorViewModel> Indicators { get; set; } = new List<EditIndicatorViewModel>();
    }
}