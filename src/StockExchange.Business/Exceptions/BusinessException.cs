using StockExchange.Business.ErrorHandling;
using System;
using System.Collections.Generic;

namespace StockExchange.Business.Exceptions
{
    /// <summary>
    /// Exception representing an error in the business layer
    /// </summary>
    public class BusinessException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="BusinessException"/>
        /// </summary>
        /// <param name="key">The property which value has caused the error</param>
        /// <param name="message">Error message</param>
        /// <param name="status">Error status</param>
        public BusinessException(string key, string message, ErrorStatus status = ErrorStatus.BusinessRuleViolation) 
            : base(message)
        {
            Errors.Add(new ValidationError(key, message));
            Status = status;
        }

        /// <summary>
        /// Creates a new instance of <see cref="BusinessException"/>
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="status">Error status</param>
        public BusinessException(string message, ErrorStatus status = ErrorStatus.BusinessRuleViolation)
        {
            Errors.Add(new ValidationError("", message));
            Status = status;
        }

        /// <summary>
        /// Errors that occurred
        /// </summary>
        public IList<ValidationError> Errors { get; } = new List<ValidationError>();

        /// <summary>
        /// Status representing a type of error
        /// </summary>
        public ErrorStatus Status { get; }
    }
}
