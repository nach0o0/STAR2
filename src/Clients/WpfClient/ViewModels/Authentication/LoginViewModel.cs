using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
using System.Net;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.Navigation;
using WpfClient.Services.Application.Notification;
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
            INavigationService navigationService, 
            INotificationService notificationService)
            : base(navigationService)
        {
            _authService = authService;

            InfoMessage = notificationService.PopMessage();
        }

        protected override async Task ExecuteSubmitAsync()
        {
            try
            {
                await _authService.LoginAsync(Email, Password);
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // We handle the specific 404 error here to provide a better message.
                ErrorMessage = "Invalid email or password.";
            }
            // All other exceptions will be caught by the ExecuteCommandAsync in ViewModelBase.
        }

        [RelayCommand]
        private void NavigateToRegister()
        {
            _navigationService.NavigateTo<RegisterViewModel>();
        }
    }
}
