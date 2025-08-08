using Permission.Domain.Authorization;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetAssignmentsForUserInScope
{
    public class GetAssignmentsForUserInScopeQueryAuthorizer : ICommandAuthorizer<GetAssignmentsForUserInScopeQuery>
    {
        public Task AuthorizeAsync(GetAssignmentsForUserInScopeQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // Fall 1: Der Benutzer fragt seine eigenen Berechtigungen ab.
            if (currentUser.UserId == command.UserId)
            {
                return Task.CompletedTask; // Immer erlaubt
            }

            // Fall 2: Ein Administrator fragt die Berechtigungen eines anderen Benutzers ab.
            var requiredPermission = AssignmentPermissions.ReadAssignments;
            var requiredScope = command.Scope;

            if (currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) &&
                permissions.Contains(requiredPermission))
            {
                return Task.CompletedTask; // Erlaubt, da der Admin die nötige Berechtigung hat.
            }

            // Wenn keine der Bedingungen zutrifft, Zugriff verweigern.
            throw new ForbiddenAccessException("You are not authorized to view assignments for this user in the specified scope.");
        }
    }
}
