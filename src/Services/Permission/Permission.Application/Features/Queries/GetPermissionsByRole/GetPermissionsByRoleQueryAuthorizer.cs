using Permission.Application.Interfaces.Persistence;
using Permission.Domain.Authorization;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetPermissionsByRole
{
    public class GetPermissionsByRoleQueryAuthorizer : ICommandAuthorizer<GetPermissionsByRoleQuery>
    {
        private readonly IRoleRepository _roleRepository;

        public GetPermissionsByRoleQueryAuthorizer(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task AuthorizeAsync(GetPermissionsByRoleQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(command.RoleId, cancellationToken)
                ?? throw new NotFoundException("Role", command.RoleId);

            var requiredPermission = RolePermissions.Read;

            // Fall 1: Die abgefragte Rolle ist eine globale Rolle (hat keinen Scope).
            if (role.Scope is null)
            {
                // Prüfe, ob der Benutzer die `role:read`-Berechtigung in IRGENDEINEM seiner Scopes besitzt.
                var hasPermissionInAnyScope = currentUser.PermissionsByScope.Values
                    .Any(permissionsList => permissionsList.Contains(requiredPermission));

                if (!hasPermissionInAnyScope)
                {
                    throw new ForbiddenAccessException($"Permission '{requiredPermission}' is required in at least one scope to view global roles.");
                }
            }
            // Fall 2: Die abgefragte Rolle ist eine gescopte Rolle.
            else
            {
                var requiredScope = role.Scope;
                // Prüfe, ob der Benutzer die Berechtigung im exakt passenden Scope hat.
                if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) || !permissions.Contains(requiredPermission))
                {
                    throw new ForbiddenAccessException($"Permission '{requiredPermission}' in scope '{requiredScope}' is required.");
                }
            }
        }
    }
}
