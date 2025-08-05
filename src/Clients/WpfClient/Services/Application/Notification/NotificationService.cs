using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Services.Application.Notification
{
    public class NotificationService : INotificationService
    {
        private string? _message;

        public void SetMessage(string message)
        {
            _message = message;
        }

        public string? PopMessage()
        {
            var messageToReturn = _message;
            _message = null; // Nachricht nach dem ersten Lesen löschen
            return messageToReturn;
        }
    }
}
