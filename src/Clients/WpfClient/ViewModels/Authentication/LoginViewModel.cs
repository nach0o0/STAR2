using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WpfClient.Messages;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.Navigation;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Authentication
{
    public partial class LoginViewModel : AuthViewModelBase
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string? _infoMessage;

        public LoginViewModel(
            IAuthService authService, 
            INavigationService navigationService)
            : base(navigationService)
        {
            _authService = authService;
        }

        public void SetInitialMessage(LoginInfoMessage message)
        {
            InfoMessage = message.Message;
        }

        protected override async Task ExecuteSubmitAsync()
        {
            await _authService.LoginAsync(Email, Password);
        }

        [RelayCommand]
        private void NavigateToRegister()
        {
            _navigationService.NavigateTo<RegisterViewModel>();
        }
    }
}
