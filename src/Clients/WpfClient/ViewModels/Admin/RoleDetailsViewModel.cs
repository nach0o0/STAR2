using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
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
    public partial class RoleDetailsViewModel : ViewModelBase
    {
        private readonly IPermissionAdminService _permissionAdminService;

        [ObservableProperty]
        private RoleModel _role;

        public bool CanEditRoleDetails { get; }
        public bool CanDeleteRoles { get; }
        public bool CanManageRolePermissions { get; }

        public event Action<Guid>? RoleDeleted;

        public RoleDetailsViewModel(
            IPermissionService permissionService,
            IPermissionAdminService permissionAdminService,
            RoleModel role,
            string scope)
        {
            _permissionAdminService = permissionAdminService;
            _role = role;

            CanEditRoleDetails = permissionService.HasPermissionInScope(PermissionKeys.RoleUpdate, scope);
            CanDeleteRoles = permissionService.HasPermissionInScope(PermissionKeys.RoleDelete, scope);
            CanManageRolePermissions = permissionService.HasPermissionInScope(PermissionKeys.RoleAssignPermission, scope);
        }

        [RelayCommand(CanExecute = nameof(CanEditRoleDetails))]
        private async Task SaveChangesAsync()
        {
            await ExecuteCommandAsync(async () =>
            {
                await _permissionAdminService.UpdateRoleAsync(Role.Id, Role.Name, Role.Description);
            });
        }

        [RelayCommand(CanExecute = nameof(CanDeleteRoles))]
        private async Task DeleteRoleAsync()
        {
            await ExecuteCommandAsync(async () =>
            {
                await _permissionAdminService.DeleteRoleAsync(Role.Id);
                RoleDeleted?.Invoke(Role.Id);
            });
        }
    }
}
