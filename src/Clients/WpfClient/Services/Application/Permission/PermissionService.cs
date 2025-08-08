using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Services.Application.UserState;

namespace WpfClient.Services.Application.Permission
{
    public class PermissionService : IPermissionService
    {
        private readonly IUserStateService _userStateService;

        public PermissionService(IUserStateService userStateService)
        {
            _userStateService = userStateService;
        }

        // Prüft, ob der Benutzer eine Berechtigung in irgendeinem Scope hat.
        public bool HasPermission(string permissionId)
        {
            if (_userStateService.CurrentUser?.PermissionsByScope is null)
            {
                return false;
            }

            return _userStateService.CurrentUser.PermissionsByScope.Values
                .SelectMany(permissions => permissions)
                .Any(p => p == permissionId);
        }

        // Prüft, ob der Benutzer eine Berechtigung in einem spezifischen Scope hat.
        public bool HasPermissionInScope(string permissionId, string scope)
        {
            if (_userStateService.CurrentUser?.PermissionsByScope is null)
            {
                return false;
            }

            return _userStateService.CurrentUser.PermissionsByScope
                .TryGetValue(scope, out var permissions) && permissions.Contains(permissionId);
        }

        public bool HasAnyPermissionInScope(IEnumerable<string> permissionIds, string scope)
        {
            if (_userStateService.CurrentUser?.PermissionsByScope is null)
            {
                return false;
            }

            // Prüft, ob der Benutzer im angegebenen Scope überhaupt Berechtigungen hat.
            if (!_userStateService.CurrentUser.PermissionsByScope.TryGetValue(scope, out var userPermissionsInScope))
            {
                return false;
            }

            // Prüft, ob es eine Schnittmenge zwischen den benötigten und den vorhandenen Berechtigungen gibt.
            return permissionIds.Any(requiredPermission => userPermissionsInScope.Contains(requiredPermission));
        }
    }
}
