using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.Navigation;
using WpfClient.Services.Application.Notification;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Authentication
{
    public partial class RegisterViewModel : AuthViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly INotificationService _notificationService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        private string _confirmPassword = string.Empty;

        public RegisterViewModel(
            IAuthService authService,
            INavigationService navigationService,
            INotificationService notificationService)
            : base(navigationService)
        {
            _authService = authService;
            _notificationService = notificationService;
        }

        protected override async Task ExecuteSubmitAsync()
        {
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return;
            }

            bool success = await _authService.RegisterAsync(Email, Password);
            if (success)
            {
                _notificationService.SetMessage("Registration successful! Please log in.");
                NavigateToLogin();
            }
            else
            {
                ErrorMessage = "Registration failed.";
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
