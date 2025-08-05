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

namespace Permission.Application.Features.Commands.CreateRole
{
    public class CreateRoleCommandAuthorizer : ICommandAuthorizer<CreateRoleCommand>
    {
        public Task AuthorizeAsync(CreateRoleCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = RolePermissions.Create;

            // Wenn ein Scope angegeben ist, wird die Berechtigung dort benötigt.
            // Wenn nicht, wird sie im globalen Scope benötigt.
            var requiredScope = command.Scope ?? PermittedScopeTypes.Global;

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to create roles in this scope is required.");
            }

            return Task.CompletedTask;
        }
    }
}
