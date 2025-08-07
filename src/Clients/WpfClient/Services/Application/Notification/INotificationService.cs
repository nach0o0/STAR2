using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Messages;

namespace WpfClient.Services.Application.Notification
{
    public interface INotificationService
    {
        void ShowMessage(string message, StatusMessageType type);
    }
}
