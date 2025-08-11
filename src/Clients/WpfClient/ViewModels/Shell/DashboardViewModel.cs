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
using WpfClient.Security;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.Permission;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Shell
{
    public partial class DashboardViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthService _authService;
        private readonly IPermissionService _permissionService;

        [ObservableProperty]
        private ViewModelBase _selectedViewModel;

        public bool CanAccessSystemAdministration { get; }

        public DashboardViewModel(IServiceProvider serviceProvider,
                                  IAuthService authService,
                                  IPermissionService permissionService)
        {
            _serviceProvider = serviceProvider;
            _authService = authService;
            _permissionService = permissionService;
            _selectedViewModel = _serviceProvider.GetRequiredService<HomeViewModel>();

            var adminPermissions = new[]
            {
                PermissionKeys.PermissionRead,
                PermissionKeys.RoleRead,
                PermissionKeys.RoleCreate,
                PermissionKeys.RoleUpdate,
                PermissionKeys.RoleDelete,
                PermissionKeys.AssignmentRead,
                PermissionKeys.RoleAssignPermission,
                PermissionKeys.PermissionAssignRole,
                PermissionKeys.PermissionAssignDirect,
                PermissionKeys.PrivilegedResetPassword
            };
            CanAccessSystemAdministration = _permissionService.HasAnyPermissionInScope(adminPermissions, "global");
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
