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
    public partial class MainViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserStateService _userStateService;

        public NotificationViewModel NotificationViewModel { get; }

        [ObservableProperty]
        private ViewModelBase _currentViewModel;

        public MainViewModel(IServiceProvider serviceProvider,
                             IUserStateService userStateService,
                             NotificationViewModel notificationViewModel)
        {
            _serviceProvider = serviceProvider;
            _userStateService = userStateService;
            NotificationViewModel = notificationViewModel;
            _userStateService.PropertyChanged += UserStateService_PropertyChanged;
            UpdateViewForCurrentState();
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
                    CurrentViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
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
