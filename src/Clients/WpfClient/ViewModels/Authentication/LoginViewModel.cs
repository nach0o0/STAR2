using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using WpfClient.Messages;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.Navigation;
using WpfClient.Services.Application.Notification;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Authentication
{
    public partial class LoginViewModel : AuthViewModelBase
    {
        private readonly IAuthService _authService;

        public LoginViewModel(
            IAuthService authService, 
            INavigationService navigationService)
            : base(navigationService)
        {
            _authService = authService;
        }

        protected override async Task ExecuteSubmitAsync()
        {
            try
            {
                await _authService.LoginAsync(Email, Password);
            }
            catch (ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var notificationService = App.Services.GetRequiredService<INotificationService>();
                notificationService.ShowMessage("Invalid email or password.", Messages.StatusMessageType.Error);
            }
        }

        [RelayCommand]
        private void NavigateToRegister()
        {
            _navigationService.NavigateTo<RegisterViewModel>();
        }
    }
}
