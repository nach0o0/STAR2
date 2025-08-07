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
    public partial class MainViewModel : ViewModelBase, IRecipient<LoginInfoMessage>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserStateService _userStateService;

        private LoginInfoMessage? _pendingLoginMessage;

        [ObservableProperty]
        private ViewModelBase _currentViewModel;

        public MainViewModel(IServiceProvider serviceProvider, IUserStateService userStateService, IMessenger messenger)
        {
            _serviceProvider = serviceProvider;
            _userStateService = userStateService;

            _userStateService.PropertyChanged += UserStateService_PropertyChanged;

            messenger.Register<LoginInfoMessage>(this);

            _currentViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
        }

        public void Receive(LoginInfoMessage message)
        {
            _pendingLoginMessage = message;
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

                    // 6. Die zwischengespeicherte Nachricht an das neue ViewModel übergeben
                    if (_pendingLoginMessage != null)
                    {
                        // Wir rufen eine Methode im LoginViewModel auf, um die Nachricht zu übergeben
                        loginViewModel.SetInitialMessage(_pendingLoginMessage);
                        _pendingLoginMessage = null; // Nachricht wurde zugestellt
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
