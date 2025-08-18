using Attendance.Domain.Authorization;
using Shared.Application.Interfaces.Infrastructure;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetWorkModelAssignmentsForEmployee
{
    public class GetWorkModelAssignmentsForEmployeeQueryAuthorizer : ICommandAuthorizer<GetWorkModelAssignmentsForEmployeeQuery>
    {
        private readonly IOrganizationServiceClient _organizationServiceClient;

        public GetWorkModelAssignmentsForEmployeeQueryAuthorizer(IOrganizationServiceClient organizationServiceClient)
        {
            _organizationServiceClient = organizationServiceClient;
        }

        public async Task AuthorizeAsync(GetWorkModelAssignmentsForEmployeeQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // Fall 1: Der Benutzer fragt seine eigenen Zuweisungen ab.
            if (currentUser.EmployeeId == command.EmployeeId)
            {
                return;
            }

            // Fall 2: Ein Administrator fragt die Zuweisungen ab.
            // Wir müssen die Organisation des angefragten Mitarbeiters kennen.
            var employeeInfo = await _organizationServiceClient.GetEmployeeInfoByUserIdAsync(currentUser.UserId, cancellationToken);
            if (employeeInfo is null || !employeeInfo.Value.OrganizationId.HasValue)
            {
                throw new ForbiddenAccessException("Cannot verify authorization for an employee without an organization.");
            }

            var requiredPermission = WorkModelPermissions.Read; // Oder eine allgemeinere Leseberechtigung
            var requiredScope = $"{PermittedScopeTypes.Organization}:{employeeInfo.Value.OrganizationId.Value}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("You are not authorized to view this employee's work model assignments.");
            }
        }
    }
}
