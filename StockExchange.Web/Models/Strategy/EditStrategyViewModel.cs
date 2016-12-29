using StockExchange.Web.Models.Indicator;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockExchange.Web.Models.Strategy
{
    public class EditStrategyViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IList<EditIndicatorViewModel> Indicators { get; set; } = new List<EditIndicatorViewModel>();
    }
}