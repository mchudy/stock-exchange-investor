using System;

namespace StockExchange.Business.Exceptions
{
    /// <summary>
    /// Exception thrown when invalid arguments are supplied when
    /// computing indicator values
    /// </summary>
    public class IndicatorArgumentException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="IndicatorArgumentException"/>
        /// </summary>
        public IndicatorArgumentException()
        {
            
        }

        /// <summary>
        /// Creates a new instance of <see cref="IndicatorArgumentException"/>
        /// </summary>
        /// <param name="message">Error message</param>
        public IndicatorArgumentException(string message) : base(message)
        {
        }
    }
}
