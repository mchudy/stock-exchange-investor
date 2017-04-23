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
        /// Indicates whether action should be taken only when all selected
        /// indicators generated a signal
        /// </summary>
        [Display(Name = "Take action only when all indicators generate a signal")]
        public bool IsConjunctiveStrategy { get; set; }

        /// <summary>
        /// Number of days in which all selected signals must occur
        /// </summary>
        [Display(Name = "Days range")]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int SignalDaysPeriod { get; set; } = 1;

        /// <summary>
        /// Chosen indicators
        /// </summary>
        public IList<EditIndicatorViewModel> Indicators { get; set; } = new List<EditIndicatorViewModel>();
    }
}