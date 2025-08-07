using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfClient.Services.Application.Auth;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.User
{
    public partial class ChangePasswordViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;

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
            IAuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand(CanExecute = nameof(CanChangePassword))]
        private async Task ChangePassword()
        {
            await ExecuteCommandAsync(async () =>
            {
                if (NewPassword != ConfirmNewPassword)
                {
                    throw new InvalidOperationException("The new passwords do not match.");
                }
                await _authService.ChangePasswordAsync(OldPassword, NewPassword);
            });
        }

        private bool CanChangePassword() =>
            !string.IsNullOrWhiteSpace(OldPassword) &&
            !string.IsNullOrWhiteSpace(NewPassword) &&
            !string.IsNullOrWhiteSpace(ConfirmNewPassword);
    }
}
