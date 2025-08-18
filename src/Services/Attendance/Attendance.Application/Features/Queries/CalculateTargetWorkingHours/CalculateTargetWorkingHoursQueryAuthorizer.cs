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

namespace Attendance.Application.Features.Queries.CalculateTargetWorkingHours
{
    public class CalculateTargetWorkingHoursQueryAuthorizer : ICommandAuthorizer<CalculateTargetWorkingHoursQuery>
    {
        private readonly IOrganizationServiceClient _organizationServiceClient;

        public CalculateTargetWorkingHoursQueryAuthorizer(IOrganizationServiceClient organizationServiceClient)
        {
            _organizationServiceClient = organizationServiceClient;
        }

        public async Task AuthorizeAsync(CalculateTargetWorkingHoursQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // Fall 1: Der Benutzer fragt seine eigenen Soll-Stunden ab.
            if (currentUser.EmployeeId == command.EmployeeId)
            {
                return;
            }

            // Fall 2: Ein Manager fragt die Soll-Stunden eines Mitarbeiters ab.
            var requiredPermission = AttendanceEntryPermissions.Read;

            // Hole die Gruppen des Ziel-Mitarbeiters vom Organization Service.
            var employeeInfo = await _organizationServiceClient.GetEmployeeInfoByEmployeeIdAsync(command.EmployeeId, cancellationToken);
            if (employeeInfo is null || !employeeInfo.Value.EmployeeGroupIds.Any())
            {
                throw new ForbiddenAccessException("Cannot verify permissions for an employee with no group memberships.");
            }

            // Prüfe, ob der Manager die Leseberechtigung in mindestens einer der Gruppen des Mitarbeiters hat.
            var hasPermissionInAnyGroupOfEmployee = employeeInfo.Value.EmployeeGroupIds
                .Any(groupId => {
                    var groupScope = $"{PermittedScopeTypes.EmployeeGroup}:{groupId}";
                    return currentUser.PermissionsByScope.TryGetValue(groupScope, out var permissions) &&
                           permissions.Contains(requiredPermission);
                });

            if (!hasPermissionInAnyGroupOfEmployee)
            {
                throw new ForbiddenAccessException("You are not authorized to calculate target hours for this employee.");
            }
        }
    }
}
