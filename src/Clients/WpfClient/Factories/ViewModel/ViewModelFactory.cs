using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Services.Application.Permission;
using WpfClient.ViewModels.Admin;

namespace WpfClient.Factories.ViewModel
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IPermissionService _permissionService;

        // Wir injizieren hier alle Abhängigkeiten, die die erstellten ViewModels benötigen.
        public ViewModelFactory(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public RoleManagementViewModel CreateRoleManagementViewModel(string scope)
        {
            // Erstellt eine neue Instanz und übergibt den Scope und die benötigten Services.
            return new RoleManagementViewModel(_permissionService, scope);
        }

        public UserManagementViewModel CreateUserManagementViewModel(string scope)
        {
            return new UserManagementViewModel(_permissionService, scope);
        }
    }
}
