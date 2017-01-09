namespace StockExchange.Business.ErrorHandling
{
    /// <summary>
    /// Represents an error thah occurred during validation in the business layer
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Creates a new instance of <see cref="ValidationError"/>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        public ValidationError(string key, string message)
        {
            Key = key;
            Message = message;
        }

        /// <summary>
        /// Name of the property that failed validation (if applicable)
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Message that should be shown to the user
        /// </summary>
        public string Message { get; }
    }
}