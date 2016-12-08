using System;

namespace StockExchange.Business.Exceptions
{
    public class IndicatorNotFoundException : Exception
    {
        public IndicatorNotFoundException(string message) : base(message)
        {
        }
    }
}