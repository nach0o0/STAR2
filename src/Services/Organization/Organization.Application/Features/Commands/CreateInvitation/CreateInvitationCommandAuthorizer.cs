using Organization.Domain.Authorization;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.CreateInvitation
{
    public class CreateInvitationCommandAuthorizer : ICommandAuthorizer<CreateInvitationCommand>
    {
        public Task AuthorizeAsync(CreateInvitationCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = InvitationPermissions.Create;

            // Der Scope, in dem die Berechtigung benötigt wird, hängt vom Ziel der Einladung ab.
            var requiredScope = command.TargetEntityType switch
            {
                InvitationTargetEntityType.Organization => $"{PermittedScopeTypes.Organization}:{command.TargetEntityId}",
                InvitationTargetEntityType.EmployeeGroup => $"{PermittedScopeTypes.EmployeeGroup}:{command.TargetEntityId}",
                _ => throw new NotSupportedException("Invitation target type is not supported.")
            };

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to create an invitation for this entity is required.");
            }

            return Task.CompletedTask;
        }
    }
}
