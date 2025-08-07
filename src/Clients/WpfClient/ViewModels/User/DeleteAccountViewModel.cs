using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.Notification;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.User
{
    public partial class DeleteAccountViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly INotificationService _notificationService;
        private readonly IMessenger _messenger;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteAccountCommand))]
        private string _confirmPassword = string.Empty;

        public DeleteAccountViewModel(
            IAuthService authService,
            INotificationService notificationService,
            IMessenger messenger)
        {
            _authService = authService;
            _notificationService = notificationService;
            _messenger = messenger;
        }

        [RelayCommand(CanExecute = nameof(CanDeleteAccount))]
        private async Task DeleteAccount()
        {
            await ExecuteCommandAsync(async () =>
            {
                _notificationService.SetMessage("Your account has been successfully deleted.");
                await _authService.DeleteMyAccountAsync(ConfirmPassword);
            });
        }

        private bool CanDeleteAccount() => !string.IsNullOrWhiteSpace(ConfirmPassword);
    }
}
