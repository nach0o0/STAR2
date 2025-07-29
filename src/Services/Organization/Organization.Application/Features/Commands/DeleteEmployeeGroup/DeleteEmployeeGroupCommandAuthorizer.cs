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

namespace Organization.Application.Features.Commands.DeleteEmployeeGroup
{
    public class DeleteEmployeeGroupCommandAuthorizer : ICommandAuthorizer<DeleteEmployeeGroupCommand>
    {
        private readonly IEmployeeGroupRepository _employeeGroupRepository;

        public DeleteEmployeeGroupCommandAuthorizer(IEmployeeGroupRepository employeeGroupRepository)
        {
            _employeeGroupRepository = employeeGroupRepository;
        }

        public async Task AuthorizeAsync(DeleteEmployeeGroupCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var employeeGroup = await _employeeGroupRepository.GetByIdAsync(command.EmployeeGroupId, cancellationToken);
            if (employeeGroup is null)
            {
                throw new NotFoundException(nameof(EmployeeGroup), command.EmployeeGroupId);
            }

            var requiredPermission = EmployeeGroupPermissions.Delete;
            var organizationScope = $"{PermittedScopeTypes.Organization}:{employeeGroup.LeadingOrganizationId}";
            var groupScope = $"{PermittedScopeTypes.EmployeeGroup}:{employeeGroup.Id}";

            var hasPermissionInOrg = currentUser.PermissionsByScope.TryGetValue(organizationScope, out var orgPermissions) &&
                                     orgPermissions.Contains(requiredPermission);

            var hasPermissionInGroup = currentUser.PermissionsByScope.TryGetValue(groupScope, out var groupPermissions) &&
                                       groupPermissions.Contains(requiredPermission);

            if (!hasPermissionInOrg && !hasPermissionInGroup)
            {
                throw new ForbiddenAccessException("Permission to delete this employee group is required.");
            }
        }
    }
}
