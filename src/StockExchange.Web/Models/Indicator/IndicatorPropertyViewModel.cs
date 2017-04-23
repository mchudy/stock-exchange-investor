using System.ComponentModel.DataAnnotations;

namespace StockExchange.Web.Models.Indicator
{
    /// <summary>
    /// View model for indicator property
    /// </summary>
    public class IndicatorPropertyViewModel
    {
        /// <summary>
        /// Property name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Property value
        /// </summary>
        public int Value { get; set; }
    }
}