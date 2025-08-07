using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.Navigation;

namespace WpfClient.ViewModels.Base
{
    public abstract partial class AuthViewModelBase : ViewModelBase
    {
        protected readonly INavigationService _navigationService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        private string _email = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        private string _password = string.Empty;

        protected AuthViewModelBase(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand(CanExecute = nameof(CanSubmit))]
        private async Task Submit()
        {
            await ExecuteCommandAsync(ExecuteSubmitAsync);
        }

        // Prüft, ob der Submit-Button aktiv sein soll.
        protected virtual bool CanSubmit()
        {
            return !string.IsNullOrWhiteSpace(Email)
                && !string.IsNullOrWhiteSpace(Password)
                && !IsLoading;
        }

        protected abstract Task ExecuteSubmitAsync();
    }
}
