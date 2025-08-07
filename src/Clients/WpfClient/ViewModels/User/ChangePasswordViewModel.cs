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
    public partial class ChangePasswordViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly INotificationService _notificationService;
        private readonly IMessenger _messenger;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangePasswordCommand))]
        private string _oldPassword = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangePasswordCommand))]
        private string _newPassword = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangePasswordCommand))]
        private string _confirmNewPassword = string.Empty;

        public ChangePasswordViewModel(
            IAuthService authService,
            INotificationService notificationService,
            IMessenger messenger)
        {
            _authService = authService;
            _notificationService = notificationService;
            _messenger = messenger;
        }

        [RelayCommand(CanExecute = nameof(CanChangePassword))]
        private async Task ChangePassword()
        {
            await ExecuteCommandAsync(async () =>
            {
                if (NewPassword != ConfirmNewPassword)
                {
                    throw new System.InvalidOperationException("The new passwords do not match.");
                }
                await _authService.ChangePasswordAsync(OldPassword, NewPassword);
                _notificationService.SetMessage("Password changed successfully. Please log in again.");
                await _authService.LogoutAsync();
            });
        }

        private bool CanChangePassword() =>
            !string.IsNullOrWhiteSpace(OldPassword) &&
            !string.IsNullOrWhiteSpace(NewPassword) &&
            !string.IsNullOrWhiteSpace(ConfirmNewPassword);
    }
}
