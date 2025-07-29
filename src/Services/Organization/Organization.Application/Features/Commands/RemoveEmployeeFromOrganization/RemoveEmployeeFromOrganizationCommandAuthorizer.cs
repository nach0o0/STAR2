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

namespace Organization.Application.Features.Commands.RemoveEmployeeFromOrganization
{
    public class RemoveEmployeeFromOrganizationCommandAuthorizer : ICommandAuthorizer<RemoveEmployeeFromOrganizationCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public RemoveEmployeeFromOrganizationCommandAuthorizer(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task AuthorizeAsync(RemoveEmployeeFromOrganizationCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(command.EmployeeId, cancellationToken);
            if (employee is null || !employee.OrganizationId.HasValue)
            {
                throw new NotFoundException(nameof(Employee), command.EmployeeId);
            }

            var requiredPermission = EmployeePermissions.RemoveFromOrganization;
            var requiredScope = $"{PermittedScopeTypes.Organization}:{employee.OrganizationId.Value}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to remove employees from this organization is required.");
            }
        }
    }
}
