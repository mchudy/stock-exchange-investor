using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Business.Indicators;

namespace StockExchange.Business.Models.Indicators
{
    public class ParameterizedIndicator
    {
        public IndicatorType IndicatorType { get; set; }
        public IList<IndicatorProperty> Properties { get; set; } 
    }
}
