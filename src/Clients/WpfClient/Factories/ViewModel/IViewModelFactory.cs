using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;
using WpfClient.ViewModels.Admin;
using WpfClient.ViewModels.Organization;

namespace WpfClient.Factories.ViewModel
{
    public interface IViewModelFactory
    {
        RoleManagementViewModel CreateRoleManagementViewModel(string scope);
        RoleListViewModel CreateRoleListViewModel(string scope);
        RoleDetailsViewModel CreateRoleDetailsViewModel(RoleModel role, string scope);
        CreateRoleViewModel CreateCreateRoleViewModel(string scope);
        ManageRolePermissionsViewModel CreateManageRolePermissionsViewModel(RoleModel role, string scope);

        UserManagementViewModel CreateUserManagementViewModel(string scope);
        UserListViewModel CreateUserListViewModel(string scope);
        UserAssignmentsViewModel CreateUserAssignmentsViewModel(UserModel user, string scope);
        AssignRoleDialogViewModel CreateAssignRoleDialogViewModel(UserModel user, string scope);
        ManageDirectPermissionsViewModel CreateManageDirectPermissionsViewModel(UserModel user, string scope, List<PermissionModel> directPermissions);

        OrganizationWorkspaceViewModel CreateOrganizationWorkspaceViewModel();
        OrganizationListViewModel CreateOrganizationListViewModel();
        OrganizationDetailsViewModel CreateOrganizationDetailsViewModel(OrganizationModel selectedOrganization);
        CreateOrganizationViewModel CreateCreateOrganizationViewModel(Guid? parentId);
    }
}
