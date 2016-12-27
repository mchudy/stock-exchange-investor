using StockExchange.Business.Indicators.Common;
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

    public class EditIndicatorViewModel
    {
        public IndicatorType Type { get; set; }
        public IList<IndicatorPropertyViewModel> Properties { get; set; } = new List<IndicatorPropertyViewModel>();
    }

    public class IndicatorPropertyViewModel
    {
        [Required]
        public string Name { get; set; }
        public int Value { get; set; }
    }
}