using StockExchange.Business.ErrorHandling;
using System;
using System.Collections.Generic;

namespace StockExchange.Business.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string key, string message, ErrorStatus status = ErrorStatus.BusinessRuleViolation) 
            : base(message)
        {
            Errors.Add(new ValidationError(key, message));
            Status = status;
        }

        public BusinessException(string message, ErrorStatus status = ErrorStatus.BusinessRuleViolation)
        {
            Errors.Add(new ValidationError("", message));
            Status = status;
        }

        public IList<ValidationError> Errors { get; } = new List<ValidationError>();
        public ErrorStatus Status { get; }
    }
}
