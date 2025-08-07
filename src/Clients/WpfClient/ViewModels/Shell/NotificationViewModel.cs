using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfClient.Messages;

namespace WpfClient.ViewModels.Shell
{
    public partial class NotificationViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _message = string.Empty;

        [ObservableProperty]
        private StatusMessageType _messageType;

        [ObservableProperty]
        private bool _isVisible;

        private CancellationTokenSource? _cts;

        public void Show(string message, StatusMessageType type)
        {
            Message = message;
            MessageType = type;

            IsVisible = true;

            // Alte Timer abbrechen
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            // Nachricht nach 5 Sekunden automatisch ausblenden
            Task.Delay(5000, _cts.Token).ContinueWith(_ =>
            {
                if (!_.IsCanceled)
                {
                    IsVisible = false;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
