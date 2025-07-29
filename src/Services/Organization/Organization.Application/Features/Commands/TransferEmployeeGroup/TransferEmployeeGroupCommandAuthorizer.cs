using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Authorization;
using Organization.Domain.Entities;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.TransferEmployeeGroup
{
    public class TransferEmployeeGroupCommandAuthorizer : ICommandAuthorizer<TransferEmployeeGroupCommand>
    {
        private readonly IEmployeeGroupRepository _employeeGroupRepository;

        public TransferEmployeeGroupCommandAuthorizer(IEmployeeGroupRepository employeeGroupRepository)
        {
            _employeeGroupRepository = employeeGroupRepository;
        }

        public async Task AuthorizeAsync(TransferEmployeeGroupCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var employeeGroup = await _employeeGroupRepository.GetByIdAsync(command.EmployeeGroupId, cancellationToken);
            if (employeeGroup is null)
            {
                throw new NotFoundException(nameof(EmployeeGroup), command.EmployeeGroupId);
            }

            var requiredPermission = EmployeeGroupPermissions.Update; // Oder eine spezifischere "transfer"-Permission

            // Definiere alle drei möglichen Scopes
            var sourceOrganizationScope = $"{PermittedScopeTypes.Organization}:{employeeGroup.LeadingOrganizationId}";
            var groupScope = $"{PermittedScopeTypes.EmployeeGroup}:{employeeGroup.Id}";
            var destinationOrganizationScope = $"{PermittedScopeTypes.Organization}:{command.DestinationOrganizationId}";

            // --- Prüfung für die Quelle (Source) ---
            // Hat der Benutzer die Berechtigung ENTWEDER in der Quell-Organisation ODER in der Gruppe selbst?
            var hasPermissionInSourceOrg = currentUser.PermissionsByScope.TryGetValue(sourceOrganizationScope, out var sourcePermissions) &&
                                            sourcePermissions.Contains(requiredPermission);

            var hasPermissionInGroup = currentUser.PermissionsByScope.TryGetValue(groupScope, out var groupPermissions) &&
                                       groupPermissions.Contains(requiredPermission);

            if (!hasPermissionInSourceOrg && !hasPermissionInGroup)
            {
                throw new ForbiddenAccessException("Permission in the source organization or on the employee group itself is required.");
            }

            // --- Prüfung für das Ziel (Destination) ---
            // Hat der Benutzer die Berechtigung in der Ziel-Organisation?
            var hasPermissionInDestination = currentUser.PermissionsByScope.TryGetValue(destinationOrganizationScope, out var destPermissions) &&
                                             destPermissions.Contains(requiredPermission);

            if (!hasPermissionInDestination)
            {
                throw new ForbiddenAccessException("Permission in the destination organization is required to transfer the employee group.");
            }
        }
    }
}
