using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.MyEmployeeProfile;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthService _authService;
        private readonly IMyEmployeeProfileService _myProfileService;

        [ObservableProperty]
        private ViewModelBase _currentViewModel;

        public MainViewModel(IServiceProvider serviceProvider, IAuthService authService, IMyEmployeeProfileService myProfileService)
        {
            _serviceProvider = serviceProvider;
            _authService = authService;
            _myProfileService = myProfileService;

            // Lausche auf Änderungen beider Services
            if (_authService is ObservableObject authObs) authObs.PropertyChanged += OnStateChanged;
            if (_myProfileService is ObservableObject profileObs) profileObs.PropertyChanged += OnStateChanged;

            UpdateNavigationState();
        }

        private void OnStateChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateNavigationState();
        }

        private void UpdateNavigationState()
        {
            if (!_authService.IsLoggedIn)
            {
                if (CurrentViewModel is not LoginViewModel)
                {
                    CurrentViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
                }
            }
            else
            {
                if (_myProfileService.CurrentProfile is null)
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
