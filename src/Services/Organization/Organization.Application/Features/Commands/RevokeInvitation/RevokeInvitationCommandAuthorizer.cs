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

namespace Organization.Application.Features.Commands.RevokeInvitation
{
    public class RevokeInvitationCommandAuthorizer : ICommandAuthorizer<RevokeInvitationCommand>
    {
        private readonly IInvitationRepository _invitationRepository;

        public RevokeInvitationCommandAuthorizer(IInvitationRepository invitationRepository)
        {
            _invitationRepository = invitationRepository;
        }

        public async Task AuthorizeAsync(RevokeInvitationCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var invitation = await _invitationRepository.GetByIdAsync(command.InvitationId, cancellationToken);
            if (invitation is null)
            {
                throw new NotFoundException(nameof(Invitation), command.InvitationId);
            }

            // Prüfung 1: Ist der Benutzer der Ersteller der Einladung?
            if (invitation.InviterEmployeeId == currentUser.EmployeeId)
            {
                return; // Autorisierung erfolgreich
            }

            // Prüfung 2: Wenn nicht, hat der Benutzer die generelle Berechtigung?
            var requiredPermission = InvitationPermissions.Revoke;

            // Ermittle den Scope dynamisch basierend auf dem Einladungsziel.
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
                return; // Autorisierung erfolgreich
            }

            // Wenn keine der Bedingungen erfüllt ist, wird der Zugriff verweigert.
            throw new ForbiddenAccessException("You are not authorized to revoke this invitation.");
        }
    }
}
