using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Messages;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.MyEmployeeProfile;
using WpfClient.Services.Application.UserState;
using WpfClient.ViewModels.Authentication;
using WpfClient.ViewModels.Base;
using WpfClient.ViewModels.User;

namespace WpfClient.ViewModels.Shell
{
    public partial class MainViewModel : ViewModelBase, IRecipient<StatusUpdateMessage>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserStateService _userStateService;

        private StatusUpdateMessage? _pendingMessage;

        [ObservableProperty]
        private ViewModelBase _currentViewModel;

        public MainViewModel(IServiceProvider serviceProvider, IUserStateService userStateService, IMessenger messenger)
        {
            _serviceProvider = serviceProvider;
            _userStateService = userStateService;

            _userStateService.PropertyChanged += UserStateService_PropertyChanged;

            messenger.Register<StatusUpdateMessage>(this);

            UpdateViewForCurrentState();
        }

        public void Receive(StatusUpdateMessage message)
        {
            _pendingMessage = message;
        }

        private void UserStateService_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Reagiere nur auf relevante Zustandsänderungen.
            if (e.PropertyName == nameof(IUserStateService.IsLoggedIn) ||
                e.PropertyName == nameof(IUserStateService.HasProfile))
            {
                UpdateViewForCurrentState();
            }
        }

        private void UpdateViewForCurrentState()
        {
            if (!_userStateService.IsLoggedIn)
            {
                if (CurrentViewModel is not LoginViewModel)
                {
                    var loginViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();

                    if (_pendingMessage != null)
                    {
                        loginViewModel.SetInitialMessage(_pendingMessage);
                        _pendingMessage = null;
                    }

                    CurrentViewModel = loginViewModel;
                }
            }
            else
            {
                if (!_userStateService.HasProfile)
                {
                    if (CurrentViewModel is not ProfileViewModel)
                    {
                        CurrentViewModel = _serviceProvider.GetRequiredService<ProfileViewModel>();
                    }
                }
                else
                {
                    if (CurrentViewModel is not DashboardViewModel)
                    {
                        CurrentViewModel = _serviceProvider.GetRequiredService<DashboardViewModel>();
                    }
                }
            }
        }
    }
}
