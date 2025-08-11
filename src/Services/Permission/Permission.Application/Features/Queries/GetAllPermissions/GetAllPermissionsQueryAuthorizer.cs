using Permission.Domain.Authorization;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetAllPermissions
{
    public class GetAllPermissionsQueryAuthorizer : ICommandAuthorizer<GetAllPermissionsQuery>
    {
        public Task AuthorizeAsync(GetAllPermissionsQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // Wir verwenden hier `role:read`, da es die grundlegende Leseberechtigung für den Admin-Bereich ist.
            var requiredPermission = RolePermissions.Read;
            var requiredScope = PermittedScopeTypes.Global;

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException($"Permission '{requiredPermission}' in scope '{requiredScope}' is required to view all permissions.");
            }

            return Task.CompletedTask;
        }
    }
}
