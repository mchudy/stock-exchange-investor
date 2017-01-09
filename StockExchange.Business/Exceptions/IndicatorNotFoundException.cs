using System;

namespace StockExchange.Business.Exceptions
{
    /// <summary>
    /// Exception thrown when there is an invocation to nonimplemented indicator
    /// </summary>
    public class IndicatorNotFoundException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="IndicatorNotFoundException"/>
        /// </summary>
        /// <param name="message">Error message</param>
        public IndicatorNotFoundException(string message) : base(message)
        {
        }
    }
}