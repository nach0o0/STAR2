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

namespace Organization.Application.Features.Commands.UpdateEmployeeGroup
{
    public class UpdateEmployeeGroupCommandAuthorizer : ICommandAuthorizer<UpdateEmployeeGroupCommand>
    {
        private readonly IEmployeeGroupRepository _employeeGroupRepository;
        private readonly IOrganizationRepository _organizationRepository;

        public UpdateEmployeeGroupCommandAuthorizer(
            IEmployeeGroupRepository employeeGroupRepository,
            IOrganizationRepository organizationRepository)
        {
            _employeeGroupRepository = employeeGroupRepository;
            _organizationRepository = organizationRepository;
        }

        public async Task AuthorizeAsync(UpdateEmployeeGroupCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var isDefaultGroup = await _organizationRepository.IsDefaultGroupOfAnyOrganizationAsync(command.EmployeeGroupId, cancellationToken);
            if (isDefaultGroup)
            {
                throw new ForbiddenAccessException("The default employee group cannot be renamed directly.");
            }

            var employeeGroup = await _employeeGroupRepository.GetByIdAsync(command.EmployeeGroupId, cancellationToken);
            if (employeeGroup is null)
            {
                throw new NotFoundException(nameof(EmployeeGroup), command.EmployeeGroupId);
            }

            var requiredPermission = EmployeeGroupPermissions.Update;
            var organizationScope = $"{PermittedScopeTypes.Organization}:{employeeGroup.LeadingOrganizationId}";
            var groupScope = $"{PermittedScopeTypes.EmployeeGroup}:{employeeGroup.Id}";

            // Prüfung 1: Hat der Benutzer die Berechtigung im Organisations-Scope?
            var hasPermissionInOrg = currentUser.PermissionsByScope.TryGetValue(organizationScope, out var orgPermissions) &&
                                     orgPermissions.Contains(requiredPermission);

            // Prüfung 2: Hat der Benutzer die Berechtigung im Gruppen-Scope?
            var hasPermissionInGroup = currentUser.PermissionsByScope.TryGetValue(groupScope, out var groupPermissions) &&
                                       groupPermissions.Contains(requiredPermission);

            // Wenn keine der beiden Prüfungen erfolgreich ist, wird der Zugriff verweigert.
            if (!hasPermissionInOrg && !hasPermissionInGroup)
            {
                throw new ForbiddenAccessException("Permission to update this employee group is required.");
            }
        }
    }
}
