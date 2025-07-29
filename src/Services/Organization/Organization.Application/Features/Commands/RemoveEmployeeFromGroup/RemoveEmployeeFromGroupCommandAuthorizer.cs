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

namespace Organization.Application.Features.Commands.RemoveEmployeeFromGroup
{
    public class RemoveEmployeeFromGroupCommandAuthorizer : ICommandAuthorizer<RemoveEmployeeFromGroupCommand>
    {
        private readonly IEmployeeGroupRepository _employeeGroupRepository;

        public RemoveEmployeeFromGroupCommandAuthorizer(IEmployeeGroupRepository employeeGroupRepository)
        {
            _employeeGroupRepository = employeeGroupRepository;
        }

        public async Task AuthorizeAsync(RemoveEmployeeFromGroupCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var employeeGroup = await _employeeGroupRepository.GetByIdAsync(command.EmployeeGroupId, cancellationToken);
            if (employeeGroup is null)
            {
                throw new NotFoundException(nameof(EmployeeGroup), command.EmployeeGroupId);
            }

            var requiredPermission = EmployeePermissions.AssignToGroup;
            var requiredScope = $"{PermittedScopeTypes.Organization}:{employeeGroup.LeadingOrganizationId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to remove employees from this group is required.");
            }
        }
    }
}
