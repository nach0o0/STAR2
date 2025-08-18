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

namespace Organization.Application.Features.Queries.GetInvitationsByTargetEntity
{
    public class GetInvitationsByTargetEntityQueryAuthorizer : ICommandAuthorizer<GetInvitationsByTargetEntityQuery>
    {
        public Task AuthorizeAsync(GetInvitationsByTargetEntityQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = InvitationPermissions.Read;
            var requiredScope = command.TargetType switch
            {
                InvitationTargetEntityType.Organization => $"{PermittedScopeTypes.Organization}:{command.TargetId}",
                InvitationTargetEntityType.EmployeeGroup => $"{PermittedScopeTypes.EmployeeGroup}:{command.TargetId}",
                _ => throw new NotSupportedException("Invitation target type is not supported.")
            };

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to read invitations for this entity is required.");
            }

            return Task.CompletedTask;
        }
    }
}
