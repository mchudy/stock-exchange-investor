namespace StockExchange.Business.ErrorHandling
{
    /// <summary>
    /// Status of a failed business operation (could be mapped to HTTP status code in AJAX requests)
    /// </summary>
    public enum ErrorStatus
    {
        DataNotFound,
        BusinessRuleViolation,
        AccessDenied
    }
}