using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Messages;
using WpfClient.ViewModels.Shell;

namespace WpfClient.Services.Application.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationViewModel _notificationViewModel;

        public NotificationService(NotificationViewModel notificationViewModel)
        {
            _notificationViewModel = notificationViewModel;
        }

        public void ShowMessage(string message, StatusMessageType type)
        {
            _notificationViewModel.Show(message, type);
        }
    }
}
