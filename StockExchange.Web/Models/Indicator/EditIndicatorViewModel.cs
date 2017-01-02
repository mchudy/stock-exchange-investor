using System.Collections.Generic;
using StockExchange.Business.Indicators.Common;

namespace StockExchange.Web.Models.Indicator
{
    public class EditIndicatorViewModel
    {
        public IndicatorType Type { get; set; }
        public string Name { get; set; }
        public IList<IndicatorPropertyViewModel> Properties { get; set; } = new List<IndicatorPropertyViewModel>();
        public bool IsSelected { get; set; }
    }
}