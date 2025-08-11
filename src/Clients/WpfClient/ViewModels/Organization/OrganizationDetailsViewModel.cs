using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Factories.ViewModel;
using WpfClient.Models;
using WpfClient.Security;
using WpfClient.Services.Application.Permission;
using WpfClient.ViewModels.Admin;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Organization
{
    public partial class OrganizationDetailsViewModel : ViewModelBase
    {
        public RoleManagementViewModel RoleManagementViewModel { get; }
        public UserManagementViewModel UserManagementViewModel { get; }

        public bool CanManageRoles { get; }
        public bool CanManageUsers { get; }

        public OrganizationDetailsViewModel(
            IViewModelFactory factory,
            IPermissionService permissionService,
            OrganizationModel selectedOrganization)
        {
            var scope = $"organization:{selectedOrganization.Id}";

            // Prüfe die Berechtigungen für die Tabs im Kontext der ausgewählten Organisation
            CanManageRoles = permissionService.HasAnyPermissionInScope(
                new[] { PermissionKeys.RoleRead, PermissionKeys.RoleCreate }, scope);

            CanManageUsers = permissionService.HasAnyPermissionInScope(
                new[] { PermissionKeys.AssignmentRead, PermissionKeys.PermissionAssignRole }, scope);

            // Erstelle die Kind-ViewModels für die Tabs nur, wenn die Berechtigung vorhanden ist
            if (CanManageRoles)
            {
                RoleManagementViewModel = factory.CreateRoleManagementViewModel(scope);
            }
            if (CanManageUsers)
            {
                UserManagementViewModel = factory.CreateUserManagementViewModel(scope);
            }
        }
    }
}
