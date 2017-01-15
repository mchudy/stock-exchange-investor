using System;

namespace StockExchange.Business.Exceptions
{
    public class IndicatorArgumentException : Exception
    {
        public IndicatorArgumentException()
        {
            
        }

        public IndicatorArgumentException(string message) : base(message)
        {
        }
    }
}
