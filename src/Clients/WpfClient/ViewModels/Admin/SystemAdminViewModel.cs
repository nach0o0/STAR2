using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Factories.ViewModel;
using WpfClient.Security;
using WpfClient.Services.Application.Permission;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Admin
{
    public partial class SystemAdminViewModel : ViewModelBase
    {
        public RoleManagementViewModel RoleManagementViewModel { get; }
        public UserManagementViewModel UserManagementViewModel { get; }

        public bool CanViewRoleManagementTab { get; }
        public bool CanViewUserManagementTab { get; }

        public SystemAdminViewModel(
            IPermissionService permissionService,
            IViewModelFactory viewModelFactory)
        {
            RoleManagementViewModel = viewModelFactory.CreateRoleManagementViewModel("global");
            UserManagementViewModel = viewModelFactory.CreateUserManagementViewModel("global");

            var canViewRoleManagementTabPermissions = new[]
            {
                PermissionKeys.RoleRead,
                PermissionKeys.RoleCreate,
                PermissionKeys.RoleUpdate,
                PermissionKeys.RoleDelete,
                PermissionKeys.RoleAssignPermission
            };
            CanViewRoleManagementTab = permissionService.HasAnyPermissionInScope(canViewRoleManagementTabPermissions, "global");

            var canViewUserManagementTabPermissions = new[]
            {
                PermissionKeys.PermissionRead,
                PermissionKeys.AssignmentRead,
                PermissionKeys.PermissionAssignRole,
                PermissionKeys.PermissionAssignDirect,
                PermissionKeys.PrivilegedResetPassword
            };
            CanViewUserManagementTab = permissionService.HasAnyPermissionInScope(canViewUserManagementTabPermissions, "global");
        }
    }
}
