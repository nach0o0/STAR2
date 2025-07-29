using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Authorization;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.ReassignOrganizationParent
{
    public class ReassignOrganizationParentCommandAuthorizer : ICommandAuthorizer<ReassignOrganizationToParentCommand>
    {
        private readonly IOrganizationRepository _organizationRepository;

        public ReassignOrganizationParentCommandAuthorizer(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task AuthorizeAsync(ReassignOrganizationToParentCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var organizationToMove = await _organizationRepository.GetByIdAsync(command.OrganizationId, cancellationToken);
            if (organizationToMove is null)
            {
                throw new NotFoundException(nameof(Organization), command.OrganizationId);
            }

            var requiredPermission = OrganizationPermissions.ReassignToParent;

            // Scope für die Quell-Organisation (wo sie aktuell ist)
            var sourceScope = organizationToMove.ParentOrganizationId.HasValue
                ? $"{PermittedScopeTypes.Organization}:{organizationToMove.ParentOrganizationId.Value}"
                : PermittedScopeTypes.Global;

            // Scope für die Ziel-Organisation (wo sie hin soll)
            var destinationScope = command.NewParentId.HasValue
                ? $"{PermittedScopeTypes.Organization}:{command.NewParentId.Value}"
                : PermittedScopeTypes.Global;

            // Prüfung 1: Berechtigung im Quell-Scope
            if (!currentUser.PermissionsByScope.TryGetValue(sourceScope, out var sourcePermissions) ||
                !sourcePermissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission in the source organization is required.");
            }

            // Prüfung 2: Berechtigung im Ziel-Scope
            if (!currentUser.PermissionsByScope.TryGetValue(destinationScope, out var destPermissions) ||
                !destPermissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission in the destination organization is required.");
            }
        }
    }
}
