using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfClient.Services.Application.Auth;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.User
{
    public partial class DeleteAccountViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteAccountCommand))]
        private string _confirmPassword = string.Empty;

        public DeleteAccountViewModel(
            IAuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand(CanExecute = nameof(CanDeleteAccount))]
        private async Task DeleteAccount()
        {
            await ExecuteCommandAsync(async () =>
            {
                await _authService.DeleteMyAccountAsync(ConfirmPassword);
            });
        }

        private bool CanDeleteAccount() => !string.IsNullOrWhiteSpace(ConfirmPassword);
    }
}
