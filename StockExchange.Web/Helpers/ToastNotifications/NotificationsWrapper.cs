using System.Collections.Generic;

namespace StockExchange.Web.Helpers.ToastNotifications
{
    /// <summary>
    /// Wrapper for toast notifications
    /// </summary>
    public class NotificationsWrapper
    {
        /// <summary>
        /// Notifications to show
        /// </summary>
        public List<ToastMessage> Messages { get; set; } = new List<ToastMessage>();

        /// <summary>
        /// Adds new notification
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="message">Message</param>
        /// <param name="toastType">Type</param>
        /// <returns>A new notification</returns>
        public ToastMessage AddToastMessage(string title, string message, ToastType toastType)
        {
            var toast = new ToastMessage
            {
                Title = title,
                Message = message,
                ToastType = toastType
            };
            Messages.Add(toast);
            return toast;
        }
    }
}