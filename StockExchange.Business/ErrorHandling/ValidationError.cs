namespace StockExchange.Business.ErrorHandling
{
    public class ValidationError
    {
        public ValidationError(string key, string message)
        {
            Key = key;
            Message = message;
        }

        public string Key { get; }
        public string Message { get; }
    }
}