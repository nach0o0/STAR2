using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Services.Application.Notification
{
    public interface INotificationService
    {
        void SetMessage(string message);
        string? PopMessage();
    }
}
