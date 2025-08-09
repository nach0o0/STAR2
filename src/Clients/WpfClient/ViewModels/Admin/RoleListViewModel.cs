using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;
using WpfClient.Security;
using WpfClient.Services.Application.Permission;
using WpfClient.Services.Application.PermissionAdmin;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Admin
{
    public partial class RoleListViewModel : ViewModelBase
    {
        private readonly string _scope;
        private readonly IPermissionAdminService _permissionAdminService;

        [ObservableProperty]
        private ObservableCollection<RoleModel> _roles = new();

        [ObservableProperty]
        private RoleModel? _selectedRole;

        public bool CanCreateRoles { get; }

        public RoleListViewModel(IPermissionService permissionService, IPermissionAdminService permissionAdminService, string scope)
        {
            _scope = scope;
            _permissionAdminService = permissionAdminService;
            CanCreateRoles = permissionService.HasPermissionInScope(PermissionKeys.RoleCreate, _scope);
            LoadRolesCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadRolesAsync()
        {
            await ExecuteCommandAsync(async () =>
            {
                var rolesList = await _permissionAdminService.GetRolesByScopeAsync(_scope);
                Roles = new ObservableCollection<RoleModel>(rolesList);
                SelectedRole = null;
            });
        }
    }
}
