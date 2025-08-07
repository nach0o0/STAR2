using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics.Metrics;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.Navigation;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Authentication
{
    public partial class RegisterViewModel : AuthViewModelBase
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        private string _confirmPassword = string.Empty;

        public RegisterViewModel(
            IAuthService authService,
            INavigationService navigationService)
            : base(navigationService)
        {
            _authService = authService;
        }

        protected override async Task ExecuteSubmitAsync()
        {
            if (Password != ConfirmPassword)
            {
                throw new InvalidOperationException("Passwords do not match.");
            }

            bool success = await _authService.RegisterAsync(Email, Password);
            if (success)
            {
                NavigateToLogin();
            }
        }

        protected override bool CanSubmit()
        {
            return base.CanSubmit() && !string.IsNullOrWhiteSpace(ConfirmPassword);
        }

        [RelayCommand]
        private void NavigateToLogin()
        {
            _navigationService.NavigateTo<LoginViewModel>();
        }
    }
}
