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

namespace Organization.Application.Features.Commands.AddEmployeeToGroup
{
    public class AddEmployeeToGroupCommandAuthorizer : ICommandAuthorizer<AddEmployeeToGroupCommand>
    {
        private readonly IEmployeeGroupRepository _employeeGroupRepository;

        public AddEmployeeToGroupCommandAuthorizer(IEmployeeGroupRepository employeeGroupRepository)
        {
            _employeeGroupRepository = employeeGroupRepository;
        }

        public async Task AuthorizeAsync(AddEmployeeToGroupCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
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
                throw new ForbiddenAccessException("Permission to add employees to groups in this organization is required.");
            }
        }
    }
}
