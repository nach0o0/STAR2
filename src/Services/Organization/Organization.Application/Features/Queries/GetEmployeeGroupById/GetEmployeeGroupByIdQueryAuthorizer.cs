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

namespace Organization.Application.Features.Queries.GetEmployeeGroupById
{
    public class GetEmployeeGroupByIdQueryAuthorizer : ICommandAuthorizer<GetEmployeeGroupByIdQuery>
    {
        private readonly IEmployeeGroupRepository _employeeGroupRepository;

        public GetEmployeeGroupByIdQueryAuthorizer(IEmployeeGroupRepository employeeGroupRepository)
        {
            _employeeGroupRepository = employeeGroupRepository;
        }

        public async Task AuthorizeAsync(GetEmployeeGroupByIdQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var employeeGroup = await _employeeGroupRepository.GetByIdAsync(command.EmployeeGroupId, cancellationToken);
            if (employeeGroup is null)
            {
                throw new NotFoundException(nameof(EmployeeGroup), command.EmployeeGroupId);
            }

            // Fall 1: Der Benutzer ist direkt Mitglied dieser Gruppe.
            if (currentUser.EmployeeGroupIds.Contains(command.EmployeeGroupId))
            {
                return;
            }

            // Fall 2: Der Benutzer hat die explizite Leseberechtigung.
            var requiredPermission = EmployeeGroupPermissions.Read;
            var groupScope = $"{PermittedScopeTypes.EmployeeGroup}:{command.EmployeeGroupId}";
            var organizationScope = $"{PermittedScopeTypes.Organization}:{employeeGroup.LeadingOrganizationId}";

            var hasPermissionInGroupScope = currentUser.PermissionsByScope.TryGetValue(groupScope, out var groupPermissions) &&
                                            groupPermissions.Contains(requiredPermission);

            var hasPermissionInOrgScope = currentUser.PermissionsByScope.TryGetValue(organizationScope, out var orgPermissions) &&
                                          orgPermissions.Contains(requiredPermission);

            if (hasPermissionInGroupScope || hasPermissionInOrgScope)
            {
                return;
            }

            throw new ForbiddenAccessException("You are not authorized to view this employee group's details.");
        }
    }
}
