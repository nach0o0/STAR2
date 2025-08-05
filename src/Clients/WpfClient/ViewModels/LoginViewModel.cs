using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Exceptions;
using WpfClient.Messages;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.Navigation;
using WpfClient.Services.Application.Notification;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels
{
    public partial class LoginViewModel : AuthViewModelBase
    {
        [ObservableProperty]
        private string? _infoMessage;

        public LoginViewModel(IAuthService authService, INavigationService navigationService, INotificationService notificationService)
            : base(authService, navigationService)
        {
            InfoMessage = notificationService.PopMessage();
        }

        // Implementiert die spezifische Login-Logik.
        protected override async Task ExecuteSubmitAsync()
        {
            // Wir brauchen eine eigene Fehlerbehandlung für den Login.
            try
            {
                var success = await _authService.LoginAsync(Email, Password);
                if (!success) // Sollte durch die Exception abgedeckt sein, aber als Sicherheit.
                {
                    ErrorMessage = "Invalid email or password.";
                }
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    ErrorMessage = "Invalid email or password.";
                }
                else
                {
                    ErrorMessage = $"An unexpected error occurred: {ex.StatusCode}";
                }
            }
        }

        // Command für den Wechsel zur Registrierungs-Ansicht.
        [RelayCommand]
        private void NavigateToRegister()
        {
            _navigationService.NavigateTo<RegisterViewModel>();
        }
    }
}
