using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.Organization;
using WpfClient.Services.Application.OrganizationAdmin;
using WpfClient.Services.Application.Permission;
using WpfClient.Services.Application.PermissionAdmin;
using WpfClient.Services.Application.UserAdmin;
using WpfClient.ViewModels.Admin;
using WpfClient.ViewModels.Organization;

namespace WpfClient.Factories.ViewModel
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IAuthService _authService;
        private readonly IPermissionService _permissionService;
        private readonly IPermissionAdminService _permissionAdminService;
        private readonly IUserAdminService _userAdminService;
        private readonly IOrganizationService _organizationService;
        private readonly IOrganizationAdminService _organizationAdminService;

        public ViewModelFactory(
            IAuthService authService,
            IPermissionService permissionService,
            IPermissionAdminService permissionAdminService,
            IUserAdminService userAdminService,
            IOrganizationService organizationService,
            IOrganizationAdminService organizationAdminService)
        {
            _authService = authService;
            _permissionService = permissionService;
            _permissionAdminService = permissionAdminService;
            _userAdminService = userAdminService;
            _organizationService = organizationService;
            _organizationAdminService = organizationAdminService;
        }

        public RoleManagementViewModel CreateRoleManagementViewModel(string scope)
        {
            return new RoleManagementViewModel(this, scope);
        }

        public RoleListViewModel CreateRoleListViewModel(string scope)
        {
            return new RoleListViewModel(_permissionService, _permissionAdminService, scope);
        }

        public RoleDetailsViewModel CreateRoleDetailsViewModel(RoleModel role, string scope)
        {
            return new RoleDetailsViewModel(_permissionService, _permissionAdminService, role, scope);
        }

        public CreateRoleViewModel CreateCreateRoleViewModel(string scope)
        {
            return new CreateRoleViewModel(_permissionAdminService, scope);
        }

        public ManageRolePermissionsViewModel CreateManageRolePermissionsViewModel(RoleModel role, string scope)
        {
            return new ManageRolePermissionsViewModel(_permissionAdminService, role, scope);
        }

        public UserManagementViewModel CreateUserManagementViewModel(string scope)
        {
            // Der Orchestrator für die Benutzerverwaltung
            return new UserManagementViewModel(this, _permissionService, scope);

        }

        public UserListViewModel CreateUserListViewModel(string scope)
        {
            // Der "Worker" für die linke Spalte (Suche/Liste)
            return new UserListViewModel(_userAdminService, scope);
        }

        public UserAssignmentsViewModel CreateUserAssignmentsViewModel(UserModel user, string scope)
        {
            // Der "Worker" für die rechte Spalte (Details)
            return new UserAssignmentsViewModel(_permissionService, _permissionAdminService, _userAdminService, user, scope);
        }

        public AssignRoleDialogViewModel CreateAssignRoleDialogViewModel(UserModel user, string scope)
        {
            return new AssignRoleDialogViewModel(_permissionAdminService, user, scope);
        }

        public ManageDirectPermissionsViewModel CreateManageDirectPermissionsViewModel(UserModel user, string scope, List<PermissionModel> directPermissions)
        {
            return new ManageDirectPermissionsViewModel(_permissionAdminService, user, scope, directPermissions);
        }

        public OrganizationWorkspaceViewModel CreateOrganizationWorkspaceViewModel()
        {
            return new OrganizationWorkspaceViewModel(this, _permissionService, _organizationService, _authService);
        }

        public OrganizationListViewModel CreateOrganizationListViewModel()
        {
            return new OrganizationListViewModel(_organizationService, _permissionService);
        }

        public OrganizationDetailsViewModel CreateOrganizationDetailsViewModel(OrganizationModel selectedOrganization)
        {
            return new OrganizationDetailsViewModel(this, _permissionService, selectedOrganization);
        }

        public CreateOrganizationViewModel CreateCreateOrganizationViewModel(Guid? parentId)
        {
            return new CreateOrganizationViewModel(_organizationAdminService, parentId);
        }
    }
}
