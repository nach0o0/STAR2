using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;
using WpfClient.Security;
using WpfClient.Services.Application.Permission;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Admin
{
    public partial class RoleDetailsViewModel : ViewModelBase
    {
        [ObservableProperty]
        private RoleModel _role;

        public bool CanEditRoleDetails { get; }
        public bool CanDeleteRoles { get; }
        public bool CanManageRolePermissions { get; }

        public RoleDetailsViewModel(IPermissionService permissionService, RoleModel role, string scope)
        {
            _role = role;
            CanEditRoleDetails = permissionService.HasPermissionInScope(PermissionKeys.RoleUpdate, scope);
            CanDeleteRoles = permissionService.HasPermissionInScope(PermissionKeys.RoleDelete, scope);
            CanManageRolePermissions = permissionService.HasPermissionInScope(PermissionKeys.RoleAssignPermission, scope);
        }
    }
}
