namespace StockExchange.Web.Helpers.ToastNotifications
{
    /// <summary>
    /// Toast notification message
    /// </summary>
    public class ToastMessage
    {
        /// <summary>
        /// Notification title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Notification message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Notification type
        /// </summary>
        public ToastType ToastType { get; set; }
    }
}