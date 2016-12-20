using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Business.Indicators;

namespace StockExchange.Business.Models.Indicators
{
    public class IndicatorDto
    {
        public IndicatorType IndicatorType { get; set; }
        public string IndicatorName { get; set; }
    }
}
