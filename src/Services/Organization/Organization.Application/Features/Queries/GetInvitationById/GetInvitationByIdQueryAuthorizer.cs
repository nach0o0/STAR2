using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Authorization;
using Organization.Domain.Entities;
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

namespace Organization.Application.Features.Queries.GetInvitationById
{
    public class GetInvitationByIdQueryAuthorizer : ICommandAuthorizer<GetInvitationByIdQuery>
    {
        private readonly IInvitationRepository _invitationRepository;

        public GetInvitationByIdQueryAuthorizer(IInvitationRepository invitationRepository)
        {
            _invitationRepository = invitationRepository;
        }

        public async Task AuthorizeAsync(GetInvitationByIdQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var invitation = await _invitationRepository.GetByIdAsync(command.InvitationId, cancellationToken);
            if (invitation is null)
            {
                throw new NotFoundException(nameof(Invitation), command.InvitationId);
            }

            // Fall 1: Der Benutzer ist der Ersteller oder der Eingeladene.
            if (currentUser.EmployeeId == invitation.InviterEmployeeId || currentUser.EmployeeId == invitation.InviteeEmployeeId)
            {
                return;
            }

            // Fall 2: Der Benutzer hat die explizite Leseberechtigung im Ziel-Scope.
            var requiredPermission = InvitationPermissions.Read;
            var requiredScope = invitation.TargetEntityType switch
            {
                InvitationTargetEntityType.Organization => $"{PermittedScopeTypes.Organization}:{invitation.TargetEntityId}",
                InvitationTargetEntityType.EmployeeGroup => $"{PermittedScopeTypes.EmployeeGroup}:{invitation.TargetEntityId}",
                _ => string.Empty
            };

            if (!string.IsNullOrEmpty(requiredScope) &&
                currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) &&
                permissions.Contains(requiredPermission))
            {
                return;
            }

            throw new ForbiddenAccessException("You are not authorized to view this invitation's details.");
        }
    }
}
