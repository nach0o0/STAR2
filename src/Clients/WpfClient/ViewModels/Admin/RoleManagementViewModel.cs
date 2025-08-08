using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Security;
using WpfClient.Services.Application.Permission;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Admin
{
    public partial class RoleManagementViewModel : ViewModelBase
    {
        private readonly string _scope;

        public bool CanCreateRoles { get; }
        public bool CanEditRoles { get; }
        public bool CanDeleteRoles { get; }

        public RoleManagementViewModel(IPermissionService permissionService, string scope)
        {
            _scope = scope;

            CanCreateRoles = permissionService.HasPermissionInScope(PermissionKeys.RoleCreate, _scope);
            CanEditRoles = permissionService.HasPermissionInScope(PermissionKeys.RoleUpdate, _scope);
            CanDeleteRoles = permissionService.HasPermissionInScope(PermissionKeys.RoleDelete, _scope);

            // TODO: Logik zum Laden der Rollen für den spezifischen _scope implementieren.
        }
    }
}
