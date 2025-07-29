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

namespace Organization.Application.Features.Commands.AssignHourlyRateToEmployee
{
    public class AssignHourlyRateToEmployeeCommandAuthorizer : ICommandAuthorizer<AssignHourlyRateToEmployeeCommand>
    {
        private readonly IEmployeeGroupRepository _employeeGroupRepository;

        public AssignHourlyRateToEmployeeCommandAuthorizer(IEmployeeGroupRepository employeeGroupRepository)
        {
            _employeeGroupRepository = employeeGroupRepository;
        }

        public async Task AuthorizeAsync(AssignHourlyRateToEmployeeCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // 1. Lade die Gruppe, um die zugehörige Organisation zu finden.
            var employeeGroup = await _employeeGroupRepository.GetByIdAsync(command.EmployeeGroupId, cancellationToken);
            if (employeeGroup is null)
            {
                throw new NotFoundException(nameof(EmployeeGroup), command.EmployeeGroupId);
            }

            var requiredPermission = EmployeePermissions.AssignHouryRate;
            var requiredScope = $"{PermittedScopeTypes.Organization}:{employeeGroup.LeadingOrganizationId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to assign hourly rates in this organization is required.");
            }
        }
    }
}
