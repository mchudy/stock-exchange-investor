using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Business.Indicators
{
    public class IndicatorValue
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }

    public class DoubleLineIndicatorValue : IndicatorValue
    {
        public decimal SecondLineValue { get; set; }
    }
}
