using System.Collections.Generic;

namespace StockExchange.Web.Helpers.ToastNotifications
{
    public class NotificationsWrapper
    {
        public List<ToastMessage> Messages { get; set; } = new List<ToastMessage>();

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