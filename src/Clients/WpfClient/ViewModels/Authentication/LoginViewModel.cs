using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Refit;
using System.Net;
using WpfClient.Messages;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.Navigation;
using WpfClient.Services.Application.Notification;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Authentication
{
    public partial class LoginViewModel : AuthViewModelBase, IRecipient<StatusUpdateMessage>
    {
        private readonly IAuthService _authService;
        private readonly IMessenger _messenger;

        [ObservableProperty]
        private string? _infoMessage;

        public LoginViewModel(
            IAuthService authService, 
            INavigationService navigationService, 
            INotificationService notificationService,
            IMessenger messenger)
            : base(navigationService)
        {
            _authService = authService;
            _messenger = messenger;
            _messenger.Register<StatusUpdateMessage>(this);

            InfoMessage = notificationService.PopMessage();
        }

        protected override async Task ExecuteSubmitAsync()
        {
            await _authService.LoginAsync(Email, Password);
        }

        public void Receive(StatusUpdateMessage message)
        {
            if (message.MessageType == StatusMessageType.Error)
            {
                ErrorMessage = message.Message;
            }
        }

        [RelayCommand]
        private void NavigateToRegister()
        {
            _navigationService.NavigateTo<RegisterViewModel>();
        }
    }
}
