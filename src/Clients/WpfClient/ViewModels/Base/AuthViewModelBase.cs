using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Exceptions;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.Navigation;

namespace WpfClient.ViewModels.Base
{
    public abstract partial class AuthViewModelBase : ViewModelBase
    {
        protected readonly IAuthService _authService;
        protected readonly INavigationService _navigationService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        private string _email = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        private string _password = string.Empty;

        [ObservableProperty]
        private string? _errorMessage;

        [ObservableProperty]
        private bool _isLoading;

        protected AuthViewModelBase(IAuthService authService, INavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;
        }

        // Asynchroner Command, der die Submit-Aktion ausführt.
        [RelayCommand(CanExecute = nameof(CanSubmit))]
        private async Task Submit()
        {
            ErrorMessage = null;
            IsLoading = true;
            try
            {
                await ExecuteSubmitAsync();
            }
            catch (HttpRequestException httpEx)
            {
                ErrorMessage = "Could not connect to the server.";
            }
            catch (ApiException ex)
            {
                // Wenn es ein Validierungsfehler ist, zeige die erste Detail-Nachricht an.
                if (ex.ValidationErrors is not null && ex.ValidationErrors.Any())
                {
                    ErrorMessage = ex.ValidationErrors.First().Value.First();
                }
                else
                {
                    ErrorMessage = "An unexpected error occurred.";
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        // Prüft, ob der Submit-Button aktiv sein soll.
        protected virtual bool CanSubmit()
        {
            return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password) && !IsLoading;
        }

        // Abstrakte Methode, die von LoginViewModel und RegisterViewModel implementiert wird.
        protected abstract Task ExecuteSubmitAsync();
    }
}
