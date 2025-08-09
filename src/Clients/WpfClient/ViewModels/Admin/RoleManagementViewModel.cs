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
    public partial class RoleManagementViewModel : ViewModelBase
    {
        private readonly string _scope;
        private readonly IPermissionAdminService _permissionAdminService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsRoleSelected))]
        private RoleModel? _selectedRole;

        public bool IsRoleSelected => SelectedRole != null;

        [ObservableProperty]
        private ObservableCollection<RoleModel> _roles = new();

        [ObservableProperty]
        private bool _isCreateFormVisible;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitCreateRoleCommand))]
        private string _newRoleName = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitCreateRoleCommand))]
        private string _newRoleDescription = string.Empty;

        public bool CanViewRoles { get; }
        public bool CanCreateRoles { get; }
        public bool CanEditRoleDetails { get; }
        public bool CanManageRolePermissions { get; }
        public bool CanDeleteRoles { get; }

        public RoleManagementViewModel(IPermissionService permissionService, IPermissionAdminService permissionAdminService, string scope)
        {
            _scope = scope;
            _permissionAdminService = permissionAdminService;

            CanViewRoles = permissionService.HasPermissionInScope(PermissionKeys.RoleRead, _scope);
            CanCreateRoles = permissionService.HasPermissionInScope(PermissionKeys.RoleCreate, _scope);
            CanEditRoleDetails = permissionService.HasPermissionInScope(PermissionKeys.RoleUpdate, _scope);
            CanManageRolePermissions = permissionService.HasPermissionInScope(PermissionKeys.RoleAssignPermission, _scope);
            CanDeleteRoles = permissionService.HasPermissionInScope(PermissionKeys.RoleDelete, _scope);

            if (CanViewRoles)
            {
                LoadRolesCommand.Execute(null);
            }
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

        [RelayCommand]
        private void ShowCreateRoleForm() => IsCreateFormVisible = true;

        [RelayCommand]
        private void CancelCreateRole()
        {
            IsCreateFormVisible = false;
            NewRoleName = string.Empty;
            NewRoleDescription = string.Empty;
        }

        [RelayCommand(CanExecute = nameof(CanSubmitCreateRole))]
        private async Task SubmitCreateRole()
        {
            await ExecuteCommandAsync(async () =>
            {
                var newRole = await _permissionAdminService.CreateRoleAsync(NewRoleName, NewRoleDescription, _scope);
                if (newRole != null)
                {
                    Roles.Add(newRole);
                    CancelCreateRole();
                }
            });
        }
        private bool CanSubmitCreateRole() => !string.IsNullOrWhiteSpace(NewRoleName);

        [RelayCommand]
        private async Task SaveChanges()
        {
            if (SelectedRole is null) return;
            await ExecuteCommandAsync(async () =>
            {
                await _permissionAdminService.UpdateRoleAsync(SelectedRole.Id, SelectedRole.Name, SelectedRole.Description);
                await LoadRolesAsync();
            });
        }

        [RelayCommand]
        private async Task DeleteRole()
        {
            if (SelectedRole is null) return;
            await ExecuteCommandAsync(async () =>
            {
                await _permissionAdminService.DeleteRoleAsync(SelectedRole.Id);
                Roles.Remove(SelectedRole);
                SelectedRole = null;
            });
        }
    }
}
