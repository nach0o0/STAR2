using Permission.Domain.Authorization;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.AssignDirectPermissionToUser
{
    public class AssignDirectPermissionToUserCommandAuthorizer : ICommandAuthorizer<AssignDirectPermissionToUserCommand>
    {
        public Task AuthorizeAsync(AssignDirectPermissionToUserCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = AssignmentPermissions.AssignDirect;
            var requiredScope = command.Scope;

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException($"Permission '{requiredPermission}' in scope '{requiredScope}' is required.");
            }

            return Task.CompletedTask;
        }
    }
}
