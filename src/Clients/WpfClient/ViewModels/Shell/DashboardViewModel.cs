using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;
using WpfClient.Services.Application.Auth;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Shell
{
    public partial class DashboardViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private ViewModelBase _selectedViewModel;

        public DashboardViewModel(IServiceProvider serviceProvider, IAuthService authService)
        {
            _serviceProvider = serviceProvider;
            _authService = authService;
            _selectedViewModel = _serviceProvider.GetRequiredService<HomeViewModel>();
        }

        [RelayCommand]
        private async Task Logout()
        {
            await _authService.LogoutAsync();
        }

        [RelayCommand]
        private void Navigate(Type? viewModelType)
        {
            if (viewModelType is null) return;
            SelectedViewModel = (ViewModelBase)_serviceProvider.GetRequiredService(viewModelType);
        }
    }
}
