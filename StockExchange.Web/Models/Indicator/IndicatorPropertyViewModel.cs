using System.ComponentModel.DataAnnotations;

namespace StockExchange.Web.Models.Indicator
{
    public class IndicatorPropertyViewModel
    {
        [Required]
        public string Name { get; set; }
        public int Value { get; set; }
    }
}