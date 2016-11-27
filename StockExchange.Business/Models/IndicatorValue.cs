using System;

namespace StockExchange.Business.Models
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
