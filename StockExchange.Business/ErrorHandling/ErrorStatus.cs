namespace StockExchange.Business.ErrorHandling
{
    /// <summary>
    /// Status of a failed business operation (could be mapped to HTTP status code in AJAX requests)
    /// </summary>
    public enum ErrorStatus
    {
        /// <summary>
        /// The requested or needed object was not found
        /// </summary>
        DataNotFound,

        /// <summary>
        /// A business rule was violated
        /// </summary>
        BusinessRuleViolation,

        /// <summary>
        /// The user did not have permission to perform the operation
        /// </summary>
        AccessDenied
    }
}