using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;
using WpfClient.Services.Application.Permission;
using WpfClient.Services.Application.PermissionAdmin;
using WpfClient.ViewModels.Admin;

namespace WpfClient.Factories.ViewModel
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IPermissionService _permissionService;
        private readonly IPermissionAdminService _permissionAdminService;

        public ViewModelFactory(IPermissionService permissionService, IPermissionAdminService permissionAdminService)
        {
            _permissionService = permissionService;
            _permissionAdminService = permissionAdminService;
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

        public UserManagementViewModel CreateUserManagementViewModel(string scope)
        {
            return new UserManagementViewModel(_permissionService, scope);
        }
    }
}
